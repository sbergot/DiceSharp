using Pidgin;
using static Pidgin.Parser;

using DiceScript.Implementation.SyntaxTree;

namespace DiceScript.Implementation.Parsing
{
    internal static class ExpressionParser
    {

        public static Parser<char, Expression> AnyExpression => OneOf(
            ArithmeticParser.ArithmeticExpression.Cast<Expression>(),
            ComplexDice.Cast<Expression>(),
            AggregationExpr.Cast<Expression>());

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
                Options = optionGroupExpr.GetValueOrDefault(),
                Name = name.GetValueOrDefault()
            };

            if (sumBonus.HasValue)
            {
                result.SumBonus = sumBonus.Value;
            }

            return result;
        }

        private static Parser<char, AggregationExpression> AggregationExpr
        {
            get
            {
                return String("aggregate ")
                    .Then(SkipWhitespaces)
                    .Then(Map((variable, options) =>
                    {
                        var result = new AggregationExpression
                        {
                            Variable = new VariableScalar { VariableName = variable },
                            Options = options
                        };
                        return result;
                    },
                    BaseParser.Variable.Before(SkipWhitespaces),
                    OptionParser.OptionGroup));
            }
        }

    }
}