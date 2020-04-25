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
            return (diceRoller) =>
            {
                var statements = tree.Statements.OfType<ExpressionStatement>();
                var expr = statements.Select(s => s.Expression).OfType<RichDiceExpression>();
                return expr.Select(e => RollRichDices(e, diceRoller)).ToList();
            };
        }

        private Roll RollRichDices(RichDiceExpression expr, DiceRoller diceRoller)
        {
            var dices = Enumerable.Range(0, expr.Dices.Number)
                .Select(i => diceRoller.Roll(expr.Dices.Faces))
                .ToList();

            var filteredDices = FilterDices(dices, expr.Filter ?? new FilterExpression { Type = FilterType.None });
            var aggrType = expr.Aggregation;
            var bonus = expr.SumBonus?.Value ?? 0;
            return new Roll
            {
                Dices = filteredDices,
                Result = ComputeResult(filteredDices, aggrType) + bonus
            };
        }

        private List<Dice> FilterDices(List<Dice> dices, FilterExpression filter)
        {
            return filter.Type switch
            {
                FilterType.None => dices,
                FilterType.Larger => dices
                    .Select(d => new Dice
                    {
                        Result = d.Result,
                        Faces = d.Faces,
                        Valid = d.Result > filter.Scalar
                    }).ToList(),
                FilterType.Smaller => dices
                    .Select(d => new Dice
                    {
                        Result = d.Result,
                        Faces = d.Faces,
                        Valid = d.Result < filter.Scalar
                    }).ToList(),
                FilterType.Equal => dices
                    .Select(d => new Dice
                    {
                        Result = d.Result,
                        Faces = d.Faces,
                        Valid = d.Result == filter.Scalar
                    }).ToList(),
                FilterType.Top => dices
                    .OrderByDescending(d => d.Result)
                    .Select((d, i) => new Dice
                    {
                        Result = d.Result,
                        Faces = d.Faces,
                        Valid = i < filter.Scalar
                    }).ToList(),
                FilterType.Bottom => dices
                    .OrderBy(d => d.Result)
                    .Select((d, i) => new Dice
                    {
                        Result = d.Result,
                        Faces = d.Faces,
                        Valid = i < filter.Scalar
                    }).ToList(),
                _ => throw new InvalidOperationException(),
            };
        }

        private int ComputeResult(List<Dice> dices, AggregationType aggregationType)
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