using DiceSharp.Implementation.SyntaxTree;
using Pidgin;
using static Pidgin.Parser;

namespace DiceSharp.Implementation.Parsing
{
    internal static class BaseParser
    {
        public static Parser<char, int> Number => Pidgin.Parser.AtLeastOnceString(Digit).Select(int.Parse);

        public static Parser<char, string> Name => LetterOrDigit.Or(Char('_')).ManyString();

        public static Parser<char, string> Variable => Char('$').Then(Name);

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

        public static Parser<char, string> QuotedString
        {
            get
            {
                return AnyCharExcept('"')
                    .ManyString()
                    .Between(Char('"'));
            }
        }
    }
}