using System;
using System.Collections.Generic;
using System.Linq;
using DiceScript.Contracts;
using DiceScript.Implementation.SyntaxTree;

namespace DiceScript.Implementation
{
    internal class Compiler
    {
        internal List<Function> CompileLib(LibraryTree lib)
        {
            var result = new List<Function>();
            foreach (var func in lib.Functions)
            {
                if (func is FunctionImplementation impl)
                {
                    result.Add(new Function
                    {
                        Spec = new FunctionSpec { Name = impl.Name, Arguments = impl.Arguments },
                        Run = (diceroller, args) =>
                        {
                            var variables = new VariableContainer(args.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value));
                            var ctx = new RunContext { Variables = variables, DiceRoller = diceroller };
                            return impl.Script.Statements.Select(script => RunStatement(script, ctx)).ToList();
                        }
                    });
                }

                if (func is PartialApplicationDeclaration partial)
                {
                    var appliedFunc = result.Single(f => f.Spec.Name == partial.AppliedFunction);
                    if (partial.ProvidedValues.Count != appliedFunc.Spec.Arguments.Count)
                    {
                        throw new InvalidScriptException(
                            "mismatch between applied func args number and provided scalars:"
                            + $"{partial.ProvidedValues.Count} and {appliedFunc.Spec.Arguments.Count}");
                    }
                    result.Add(new Function
                    {
                        Spec = new FunctionSpec { Name = partial.Name, Arguments = partial.Arguments },
                        Run = (diceroller, args) =>
                        {
                            var outvariables = new VariableContainer(args.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value));
                            var innerArgs = new Dictionary<string, int>();
                            for (var idx = 0; idx < appliedFunc.Spec.Arguments.Count; idx++)
                            {
                                var value = outvariables.GetScalarValue(partial.ProvidedValues[idx]);
                                innerArgs[appliedFunc.Spec.Arguments[idx]] = value;
                            }
                            return appliedFunc.Run(diceroller, innerArgs);
                        }
                    });
                }
            }
            return result;
        }

        internal Func<DiceRoller, IList<Result>> CompileScript(Script tree)
        {
            return (diceRoller) =>
            {
                var ctx = new RunContext { Variables = new VariableContainer(), DiceRoller = diceRoller };
                return tree.Statements.Select(script => RunStatement(script, ctx)).ToList();
            };
        }

        private static Result RunStatement(Statement statement, RunContext ctx)
        {
            _ = statement ?? throw new ArgumentNullException(nameof(statement));

            if (statement is ExpressionStatement exprStmt)
            {
                return RunExpression(exprStmt.Expression, ctx);
            }

            if (statement is AssignementStatement assignStmt)
            {
                var roll = RunExpression(assignStmt.Expression, ctx);
                if (assignStmt.Type == AssignementType.Number)
                {
                    ctx.Variables.SetVariable(assignStmt.VariableName, roll.Result);
                    return roll;
                }
                else
                {
                    var rollResult = roll as RollResult;
                    if (rollResult == null)
                    {
                        throw new InvalidScriptException("trying to save dice without a dice expression");
                    }
                    ctx.Variables.SetVariable(assignStmt.VariableName, roll);
                    return new DiceResult
                    {
                        Dices = rollResult.Dices,
                        Name = rollResult.Name,
                        Description = rollResult.Description
                    };
                }
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

        private static ValueResult RunExpression(Expression expr, RunContext ctx)
        {
            if (expr is DiceExpression diceexpr)
            {
                return RunDiceExpression(diceexpr, ctx);
            }

            if (expr is CalcExpression calcexpr)
            {
                return RunCalcExpression(calcexpr, ctx);
            }

            if (expr is AggregationExpression aggregation)
            {
                return RunAggregationExpression(aggregation, ctx);
            }

            throw new InvalidOperationException($"Uknown expression type: {expr.GetType()}");
        }

        private static ValueResult RunAggregationExpression(AggregationExpression expr, RunContext ctx)
        {
            var dice = ctx.Variables.GetVariableValue<RollResult>(expr.Variable);
            var options = MergeOptions(expr.Options);
            var filteredDices = FilterDices(
                dice.Dices,
                options.Filter ?? new FilterOption { Type = FilterType.None },
                options.Ranking ?? new RankingOption { Type = RankingType.None },
                ctx.Variables);
            var result = ComputeResult(filteredDices, options.Aggregation);
            return new ValueResult { Result = result, Name = expr.Name };
        }

        private static ValueResult RunCalcExpression(CalcExpression expr, RunContext ctx)
        {
            var leftVal = ctx.Variables.GetScalarValue(expr.LeftValue);
            var rightVal = ctx.Variables.GetScalarValue(expr.RightValue);
            var result = expr.Operator switch
            {
                SignType.Plus => leftVal + rightVal,
                SignType.Minus => leftVal - rightVal,
                SignType.Multiply => leftVal * rightVal,
                _ => throw new InvalidOperationException()
            };
            return new ValueResult
            {
                Result = result,
                Name = expr.Name
            };
        }

        private static DiceOptions MergeOptions(OptionGroup options)
        {
            var result = new DiceOptions();
            if (options == null) { return result; }
            foreach (var option in options.Options)
            {
                if (option is FilterOption filter)
                {
                    result.Filter = filter;
                }

                if (option is RankingOption ranking)
                {
                    result.Ranking = ranking;
                }

                if (option is AggregateOption aggregate)
                {
                    result.Aggregation = aggregate.Type;
                }

                if (option is ExplodingOption)
                {
                    result.Exploding = true;
                }
            }
            return result;
        }

        private static RollResult RunDiceExpression(DiceExpression expr, RunContext ctx)
        {
            var diceNbr = ctx.Variables.GetScalarValue(expr.Dices.Number);
            if (diceNbr < 1)
            {
                throw new InvalidScriptException("Dice number must be positive");
            }
            var faceNbr = ctx.Variables.GetScalarValue(expr.Dices.Faces);
            var options = MergeOptions(expr.Options);
            var dices = Enumerable.Range(0, diceNbr)
                .Select(i => ctx.DiceRoller.Roll(faceNbr, options.Exploding))
                .ToList();

            var filteredDices = FilterDices(
                dices,
                options.Filter ?? new FilterOption { Type = FilterType.None },
                options.Ranking ?? new RankingOption { Type = RankingType.None },
                ctx.Variables);
            var aggrType = options.Aggregation;
            var bonus = expr.SumBonus != null
                ? ctx.Variables.GetScalarValue(expr.SumBonus.Scalar) * GetSignFactor(expr.SumBonus.Sign)
                : 0;
            return new RollResult
            {
                Description = new RollDescription
                {
                    Faces = faceNbr,
                    Number = diceNbr,
                    Bonus = bonus,
                    Exploding = options.Exploding,
                },
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