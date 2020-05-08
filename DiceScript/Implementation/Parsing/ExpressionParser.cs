using Pidgin;
using static Pidgin.Parser;

using DiceScript.Implementation.SyntaxTree;

namespace DiceScript.Implementation.Parsing
{
    internal static class ExpressionParser
    {

        public static Parser<char, Expression> AnyExpression => OneOf(
            ArithmeticParser.ArithmeticExpression.Cast<Expression>(),
            ComplexDice.Cast<Expression>());

        public static Parser<char, DiceExpression> ComplexDice
        {
            get
            {
                var rollCommand = BaseParser.QuotedString
                    .Optional()
                    .Between(Try(String("roll ")).Then(SkipWhitespaces), SkipWhitespaces);
                return Map(
                    ComputeDiceExpression,
                    rollCommand,
                    DiceParser.Dice,
                    DiceParser.SumBonus.Optional(),
                    SkipWhitespaces.Then(OptionParser.OptionGroup.Optional())
                );
            }
        }

        private static DiceExpression ComputeDiceExpression(
            Maybe<string> name,
            DiceDeclaration diceExpr,
            Maybe<SumBonusDeclaration> sumBonus,
            Maybe<OptionGroup> optionGroupExpr)
        {
            var result = new DiceExpression
            {
                Dices = diceExpr,
                Aggregation = AggregationType.Sum,
                Name = name.GetValueOrDefault()
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