using Pidgin;
using static Pidgin.Parser;

namespace DiceSharp.Implementation.Parsing
{
    internal class BaseParser
    {
        public static Parser<char, int> Number => Pidgin.Parser.AtLeastOnceString(Digit).Select(int.Parse);
    }
}