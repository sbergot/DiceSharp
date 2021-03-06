using Pidgin;
using static Pidgin.Parser;

using DiceScript.Implementation.SyntaxTree;

namespace DiceScript.Implementation.Parsing
{
    internal static class DiceParser
    {
        public static Parser<char, DiceDeclaration> Dice
        {
            get
            {
                var singleDice = CIChar('D')
                    .Then(BaseParser.Scalar)
                    .Select(n => new DiceDeclaration { Number = new ConstantScalar { Value = 1 }, Faces = n });
                return Map(
                    (n, dice) =>
                    {
                        if (n.HasValue)
                        {
                            dice.Number = n.Value;
                        }
                        return dice;
                    },
                    BaseParser.Scalar.Optional(),
                    singleDice);
            }
        }

        public static Parser<char, SumBonusDeclaration> SumBonus
        {
            get
            {
                var operation = Char('+').Or(Char('-'));
                return Map(
                    (op, bonus) => new SumBonusDeclaration
                    {
                        Scalar = bonus,
                        Sign = op == '+' ? SignType.Plus : SignType.Minus,
                    },
                    operation,
                    BaseParser.Scalar);
            }
        }
    }
}