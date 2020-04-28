using Pidgin;
using static Pidgin.Parser;

using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Implementation.Parsing
{
    internal static class ExpressionParser
    {

        public static Parser<char, Expression> AnyExpression => OneOf(Try(ComplexDice.Cast<Expression>()));

        public static Parser<char, DiceExpression> ComplexDice
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

        private static DiceExpression ComputeDiceExpression(
            DiceDeclaration diceExpr,
            Maybe<SumBonusDeclaration> sumBonus,
            Maybe<OptionGroup> optionGroupExpr)
        {
            var result = new DiceExpression
            {
                Dices = diceExpr,
                Aggregation = AggregationType.Sum,
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

                    if (option is RankingOption ranking)
                    {
                        result.Ranking = ranking;
                    }

                    if (option is AggregateOption aggregate)
                    {
                        result.Aggregation = aggregate.Type;
                    }

                    if (option is NameOption name)
                    {
                        result.Name = name.Name;
                    }

                    if (option is ExplodingOption)
                    {
                        result.Exploding = true;
                    }
                }
            }

            return result;
        }
    }
}