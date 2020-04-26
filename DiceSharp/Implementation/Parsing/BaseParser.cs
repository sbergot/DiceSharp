using DiceSharp.Implementation.SyntaxTree;
using Pidgin;
using static Pidgin.Parser;

namespace DiceSharp.Implementation.Parsing
{
    internal static class BaseParser
    {
        public static Parser<char, int> Number => Pidgin.Parser.AtLeastOnceString(Digit).Select(int.Parse);

        public static Parser<char, string> Variable
        {
            get
            {
                return Char('$')
                    .Then(LetterOrDigit.Or(Char('_')).ManyString());
            }
        }

        public static Parser<char, Scalar> Scalar
        {
            get
            {
                return OneOf(
                    Try(Number.Select(n => new ConstantScalar { Value = n })).Cast<Scalar>(),
                    Try(Variable.Select(n => new VariableScalar { VariableName = n })).Cast<Scalar>()
                );
            }
        }
    }
}