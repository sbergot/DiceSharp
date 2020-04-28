using System;
using System.Collections.Generic;
using System.Linq;
using DiceSharp.Contracts;
using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Implementation
{
    internal class Compiler
    {
        internal Func<DiceRoller, IList<Result>> Compile(Script tree)
        {
            return (diceRoller) =>
            {
                var ctx = new RunContext { Variables = new VariableContainer(), DiceRoller = diceRoller };
                return tree.Statements.Select(s => RunStatement(s, ctx)).ToList();
            };
        }

        private static Result RunStatement(Statement statement, RunContext ctx)
        {
            _ = statement ?? throw new ArgumentNullException(nameof(statement));

            if (statement is ExpressionStatement exprStmt)
            {
                return RunDiceExpression(exprStmt.Expression as DiceExpression, ctx);
            }

            if (statement is AssignementStatement assignStmt)
            {
                var roll = RunDiceExpression(assignStmt.Expression as DiceExpression, ctx);
                ctx.Variables.SetVariable(assignStmt.VariableName, roll.Result);
                return roll;
            }

            if (statement is RangeMappingStatement rangeStmt)
            {
                var result = RunRangeMapping(ctx, rangeStmt);
                return new PrintResult { Value = result };
            }

            throw new InvalidOperationException($"Uknown statement type: {statement.GetType()}");
        }

        private static string RunRangeMapping(RunContext ctx, RangeMappingStatement rangeStmt)
        {
            var scalarValue = ctx.Variables.GetScalarValue(rangeStmt.Scalar);
            foreach (var range in rangeStmt.Ranges)
            {
                var filter = range.Filter;
                var filterScalarValue = ctx.Variables.GetScalarValue(filter.Scalar ?? new ConstantScalar { Value = 0 });
                var match = Compare(filter.Type, filterScalarValue, scalarValue);
                if (match) { return range.Value; }
            }
            return null;
        }

        private static RollResult RunDiceExpression(DiceExpression expr, RunContext ctx)
        {
            var dices = Enumerable.Range(0, expr.Dices.Number)
                .Select(i => ctx.DiceRoller.Roll(expr.Dices.Faces, expr.Exploding))
                .ToList();

            var filteredDices = FilterDices(
                dices,
                expr.Filter ?? new FilterOption { Type = FilterType.None },
                expr.Ranking ?? new RankingOption { Type = RankingType.None },
                ctx.Variables);
            var aggrType = expr.Aggregation;
            var bonus = expr.SumBonus != null
                ? ctx.Variables.GetScalarValue(expr.SumBonus.Scalar) * GetSignFactor(expr.SumBonus.Sign)
                : 0;
            return new RollResult
            {
                Dices = filteredDices,
                Result = ComputeResult(filteredDices, aggrType) + bonus,
                Name = expr.Name
            };
        }

        private static int GetSignFactor(SignType sign)
        {
            return sign switch
            {
                SignType.Plus => 1,
                SignType.Minus => -1,
                _ => throw new InvalidOperationException()
            };
        }

        private static List<Dice> FilterDices(List<Dice> dices, FilterOption filter, RankingOption ranking, VariableContainer variables)
        {
            List<Dice> filteredDices = ApplyFilter(dices, filter, variables);

            return ApplyRanking(ranking, variables, filteredDices);
        }

        private static List<Dice> ApplyRanking(RankingOption ranking, VariableContainer variables, List<Dice> filteredDices)
        {
            var scalarValue = ranking.Type != RankingType.None ? variables.GetScalarValue(ranking.Scalar) : 0;
            return ranking.Type switch
            {
                RankingType.None => filteredDices,
                RankingType.Top => filteredDices
                    .OrderByDescending(d => d.Result)
                    .Select((d, i) => new Dice
                    {
                        Result = d.Result,
                        Faces = d.Faces,
                        Valid = i < scalarValue
                    }).ToList(),
                RankingType.Bottom => filteredDices
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

        private static List<Dice> ApplyFilter(List<Dice> dices, FilterOption filter, VariableContainer variables)
        {
            var scalarValue = filter.Type != FilterType.None ? variables.GetScalarValue(filter.Scalar) : 0;
            return dices
                .Select(d => new Dice
                {
                    Result = d.Result,
                    Faces = d.Faces,
                    Valid = Compare(filter.Type, scalarValue, d.Result)
                }).ToList();
        }

        private static bool Compare(FilterType filterType, int filterValue, int valueToCompare)
        {
            return filterType switch
            {
                FilterType.None => true,
                FilterType.Larger => valueToCompare > filterValue,
                FilterType.Smaller => valueToCompare < filterValue,
                FilterType.Equal => valueToCompare == filterValue,
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