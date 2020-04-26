using Pidgin;
using static Pidgin.Parser;

using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Implementation.Parsing
{
    internal static class ExpressionParser
    {

        public static Parser<char, Expression> AnyExpression => OneOf(Try(ComplexDice.Cast<Expression>()));

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
            DiceDeclaration diceExpr,
            Maybe<SumBonusDeclaration> sumBonus,
            Maybe<OptionGroup> optionGroupExpr)
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
                    if (option is FilterOption filter)
                    {
                        result.Filter = filter;
                    }

                    if (option is AggregateOption aggregate)
                    {
                        result.Aggregation = aggregate.Type;
                    }
                }
            }

            return result;
        }
    }
}