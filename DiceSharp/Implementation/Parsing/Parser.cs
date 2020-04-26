using System;
using System.Linq;

using Pidgin;
using static Pidgin.Parser;

using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Implementation.Parsing
{
    internal class Parser
    {
        internal Ast Parse(string program)
        {
            var lines = program.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return new Ast
            {
                Statements = lines.Select(l => StatementParser.AnyStatement.ParseOrThrow(l)).ToList()
            };
        }
    }
}