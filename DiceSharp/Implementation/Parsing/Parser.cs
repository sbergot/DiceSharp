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

        internal Library ParseLibrary(string program)
        {
            return LibraryParser.LibraryBlock.ParseOrThrow(program);
        }
    }
}