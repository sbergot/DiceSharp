using System.Collections.Generic;
using System.Linq;
using DiceScript.Implementation.SyntaxTree;
using Pidgin;
using static Pidgin.Parser;

namespace DiceScript.Implementation.Parsing
{
    internal static class BaseParser
    {
        public static Parser<char, int> Number => Pidgin.Parser.AtLeastOnceString(Digit).Select(int.Parse);

        public static Parser<char, string> Name => LetterOrDigit.Or(Char('_')).ManyString();

        public static Parser<char, string> Variable => Char('$').Then(Name).Before(Char('$').Optional());

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

        public static Parser<char, List<T>> List<T>(Parser<char, T> itemParser)
        {
            return itemParser
                .Separated(Char(',').Between(SkipWhitespaces))
                .Between(Char('('), Char(')'))
                .Select(s => s.ToList());
        }
    }
}