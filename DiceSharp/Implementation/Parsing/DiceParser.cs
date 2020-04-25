using Pidgin;
using static Pidgin.Parser;
using static DiceSharp.Implementation.Parsing.BaseParser;

using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Implementation.Parsing
{
    internal class DiceParser
    {
        public static Parser<char, DiceExpression> Dice
        {
            get
            {
                var singleDice = Char('D')
                    .Then(Number)
                    .Select(n => new DiceExpression { Number = 1, Faces = n });
                var multipleDice = Map((n, dice) => new DiceExpression { Number = n, Faces = dice.Faces }, Number, singleDice);
                return multipleDice.Or(singleDice);
            }
        }

        public static Parser<char, SumBonusExpression> SumBonus
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
    }
}