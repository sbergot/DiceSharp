using Pidgin;
using static Pidgin.Parser;
using static DiceSharp.Implementation.Parsing.BaseParser;

using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Implementation.Parsing
{
    internal static class DiceParser
    {
        public static Parser<char, DiceDeclaration> Dice
        {
            get
            {
                var singleDice = Char('D')
                    .Then(Number)
                    .Select(n => new DiceDeclaration { Number = 1, Faces = n });
                var multipleDice = Map((n, dice) => new DiceDeclaration { Number = n, Faces = dice.Faces }, Number, singleDice);
                return multipleDice.Or(singleDice);
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