using System.Collections.Generic;

namespace DiceSharp.Implementation.SyntaxTree
{
    internal class FunctionDeclaration
    {
        public string Name { get; set; }
        public List<string> Arguments { get; set; }
        public Script Script { get; set; }
    }
}