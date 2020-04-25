using System;
using System.Linq;

using Pidgin;
using static Pidgin.Parser;

using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Implementation.Parsing
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

        private static Parser<char, Expression> AnyExpression => OneOf(Try(ComplexDice.Cast<Expression>()));

        private static Parser<char, RichDiceExpression> ComplexDice
        {
            get
            {
                return Map(
                    ComputeDiceExpression,
                    DiceParser.Dice,
                    DiceParser.SumBonus.Optional(),
                    SkipWhitespaces.Then(OptionParser.OptionGroup).Optional()
                );
            }
        }

        private static RichDiceExpression ComputeDiceExpression(
            DiceExpression diceExpr,
            Maybe<SumBonusExpression> sumBonus,
            Maybe<OptionGroupExpression> optionGroupExpr)
        {
            var result = new RichDiceExpression
            {
                Dices = diceExpr,
                Aggregation = AggregationType.Sum,
                Filter = null
            };

            if (sumBonus.HasValue)
            {
                result.SumBonus = sumBonus.Value;
            }

            if (optionGroupExpr.HasValue)
            {
                foreach (var option in optionGroupExpr.Value.Options)
                {
                    if (option is FilterExpression filter)
                    {
                        result.Filter = filter;
                    }

                    if (option is AggregateExpression aggregate)
                    {
                        result.Aggregation = aggregate.Type;
                    }
                }
            }

            return result;
        }
    }
}