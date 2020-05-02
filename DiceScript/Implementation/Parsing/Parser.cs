using Pidgin;

using DiceScript.Implementation.SyntaxTree;

namespace DiceScript.Implementation.Parsing
{
    internal class Parser
    {
        private static Parser<char, Script> ScriptParser = StatementsParser.ScriptParser;
        private static Parser<char, LibraryTree> LibParser = LibraryParser.LibraryBlock;
        internal Script ParseScript(string program)
        {
            return ScriptParser.ParseOrThrow(program);
        }

        internal LibraryTree ParseLibrary(string program)
        {
            return LibParser.ParseOrThrow(program);
        }
    }
}