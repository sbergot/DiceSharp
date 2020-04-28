using System.Linq;

using Pidgin;
using static Pidgin.Parser;

using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Implementation.Parsing
{
    internal class Parser
    {
        internal Script Parse(string program)
        {
            return ScriptParser.ParseOrThrow(program);
        }

        private static Parser<char, Script> ScriptParser
        {
            get
            {
                return StatementParser.AnyStatement
                    .Separated(Char(';'))
                    .Select(s => new Script
                    {
                        Statements = s.ToList()
                    });
            }
        }
    }
}