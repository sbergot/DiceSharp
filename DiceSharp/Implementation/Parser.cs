using System;
using System.Collections.Generic;
using System.Linq;

using Pidgin;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;

using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Implementation
{
    internal class Parser
    {
        internal Ast Parse(string program)
        {
            var lines = program.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return new Ast
            {
                Statements = lines.Select(ParseStatement).ToList()
            };
        }

        private Statement ParseStatement(string line)
        {
            return new ExpressionStatement
            {
                Expression = AnyExpression.ParseOrThrow(line)
            };
        }

        private static Parser<char, int> Number => Pidgin.Parser.AtLeastOnceString(Digit).Select(int.Parse);

        private static Parser<char, DiceExpression> Dice
        {
            get
            {
                var singleDice = Char('D').Then(Number).Select(n => new DiceExpression { Number = 1, Faces = n });
                var multipleDice = Map((n, dice) => new DiceExpression { Number = n, Faces = dice.Faces }, Number, singleDice);
                return multipleDice.Or(singleDice);
            }
        }

        private static Parser<char, SumBonusExpression> SumBonus
        {
            get
            {
                var operation = Char('+').Or(Char('-'));
                return Map(
                    (op, bonus) => new SumBonusExpression
                    {
                        Value = op == '+' ? bonus : -bonus,
                    },
                    operation,
                    Number);
            }
        }

        private static Parser<char, Expression> AnyExpression
        {
            get
            {
                return OneOf(Try(ComplexDice.Cast<Expression>()));
            }
        }

        private static Parser<char, FilterExpression> Filter
        {
            get
            {
                var filterTypes = new Dictionary<string, FilterType>
                {
                    { "bot", FilterType.Bottom },
                    { "=", FilterType.Equal },
                    { ">", FilterType.Larger },
                    { "<", FilterType.Smaller },
                    { "top", FilterType.Top },
                };
                var type = OneOf(filterTypes.Keys.Select(String)).Select(s => filterTypes[s]);
                return Map(
                    (_1, t, n, _2) => new FilterExpression { Type = t, Scalar = n },
                    Char('('),
                    type,
                    Number,
                    Char(')'));
            }
        }

        private static Parser<char, AggregateExpression> Aggregate
        {
            get
            {
                var aggreationTypes = new Dictionary<string, AggregationType>
                {
                    { "sum", AggregationType.Sum },
                    { "count", AggregationType.Count },
                    { "max", AggregationType.Max },
                    { "min", AggregationType.Min },
                };
                var type = OneOf(aggreationTypes.Keys.Select(s => Try(String(s)))).Select(s => aggreationTypes[s]);
                return SkipWhitespaces
                    .Then(Char('|'))
                    .Then(SkipWhitespaces)
                    .Then(type)
                    .Select(t => new AggregateExpression { Type = t });
            }
        }

        private static Parser<char, RichDiceExpression> ComplexDice
        {
            get
            {
                return Map(
                    (dice, sumBonus, filter, aggregation) => new RichDiceExpression
                    {
                        Dices = dice,
                        Aggregation = aggregation.GetValueOrDefault(),
                        Filter = filter.GetValueOrDefault(),
                        SumBonus = sumBonus.GetValueOrDefault(),
                    },
                    Dice,
                    SumBonus.Optional(),
                    Filter.Optional(),
                    Aggregate.Optional()
                );
            }
        }
    }
}