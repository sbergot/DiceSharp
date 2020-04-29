using Pidgin;

using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Implementation.Parsing
{
    internal class Parser
    {
        internal Script ParseScript(string program)
        {
            return StatementsParser.ScriptParser.ParseOrThrow(program);
        }

        internal LibraryTree ParseLibrary(string program)
        {
            return LibraryParser.LibraryBlock.ParseOrThrow(program);
        }
    }
}