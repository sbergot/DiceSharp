using System;
using System.Collections.Generic;
using System.Linq;
using DiceSharp.Contracts;
using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Implementation
{
    internal class Compiler
    {
        internal Func<DiceRoller, IList<Roll>> Compile(Ast tree)
        {
            var variables = new Dictionary<string, int>();
            return (diceRoller) =>
            {
                var ctx = new RunContext { Variables = variables, DiceRoller = diceRoller };
                return tree.Statements.Select(s => RunStatement(s, ctx)).ToList();
            };
        }

        private static Roll RunStatement(Statement statement, RunContext ctx)
        {
            _ = statement ?? throw new ArgumentNullException(nameof(statement));

            if (statement is ExpressionStatement exprStmt)
            {
                return RollRichDices(exprStmt.Expression as RichDiceExpression, ctx);
            }

            if (statement is AssignementStatement assignStmt)
            {
                var roll = RollRichDices(assignStmt.Expression as RichDiceExpression, ctx);
                ctx.Variables[assignStmt.VariableName] = roll.Result;
                return roll;
            }

            throw new InvalidOperationException($"Uknown statement type: {statement.GetType()}");
        }

        private static Roll RollRichDices(RichDiceExpression expr, RunContext ctx)
        {
            var dices = Enumerable.Range(0, expr.Dices.Number)
                .Select(i => ctx.DiceRoller.Roll(expr.Dices.Faces))
                .ToList();

            var filteredDices = FilterDices(
                dices,
                expr.Filter ?? new FilterOption { Type = FilterType.None },
                ctx.Variables);
            var aggrType = expr.Aggregation;
            var bonus = expr.SumBonus?.Value ?? 0;
            return new Roll
            {
                Dices = filteredDices,
                Result = ComputeResult(filteredDices, aggrType) + bonus
            };
        }

        private static int GetScalarValue(Dictionary<string, int> variables, Scalar scalar)
        {
            _ = scalar ?? throw new ArgumentNullException(nameof(scalar));

            if (scalar is ConstantScalar constant)
            {
                return constant.Value;
            }

            if (scalar is VariableScalar variable)
            {
                return variables[variable.VariableName];
            }

            throw new InvalidOperationException($"Unknown scalar type: {scalar.GetType()}");
        }

        private static List<Dice> FilterDices(List<Dice> dices, FilterOption filter, Dictionary<string, int> variables)
        {
            var scalarValue = filter.Type != FilterType.None ? GetScalarValue(variables, filter.Scalar) : 0;
            return filter.Type switch
            {
                FilterType.None => dices,
                FilterType.Larger => dices
                    .Select(d => new Dice
                    {
                        Result = d.Result,
                        Faces = d.Faces,
                        Valid = d.Result > scalarValue
                    }).ToList(),
                FilterType.Smaller => dices
                    .Select(d => new Dice
                    {
                        Result = d.Result,
                        Faces = d.Faces,
                        Valid = d.Result < scalarValue
                    }).ToList(),
                FilterType.Equal => dices
                    .Select(d => new Dice
                    {
                        Result = d.Result,
                        Faces = d.Faces,
                        Valid = d.Result == scalarValue
                    }).ToList(),
                FilterType.Top => dices
                    .OrderByDescending(d => d.Result)
                    .Select((d, i) => new Dice
                    {
                        Result = d.Result,
                        Faces = d.Faces,
                        Valid = i < scalarValue
                    }).ToList(),
                FilterType.Bottom => dices
                    .OrderBy(d => d.Result)
                    .Select((d, i) => new Dice
                    {
                        Result = d.Result,
                        Faces = d.Faces,
                        Valid = i < scalarValue
                    }).ToList(),
                _ => throw new InvalidOperationException(),
            };
        }

        private static int ComputeResult(List<Dice> dices, AggregationType aggregationType)
        {
            var init = aggregationType switch
            {
                AggregationType.Count => 0,
                AggregationType.Max => int.MinValue,
                AggregationType.Min => int.MaxValue,
                AggregationType.Sum => 0,
                _ => throw new InvalidOperationException()
            };
            return dices
                .Where(d => d.Valid)
                .Select(d => d.Result)
                .Aggregate(init, (acc, val) => aggregationType switch
                {
                    AggregationType.Count => acc + 1,
                    AggregationType.Max => Math.Max(acc, val),
                    AggregationType.Min => Math.Min(acc, val),
                    AggregationType.Sum => acc + val,
                    _ => throw new InvalidOperationException()
                });
        }
    }
}