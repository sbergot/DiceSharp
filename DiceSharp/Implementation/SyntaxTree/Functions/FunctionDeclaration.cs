using System.Collections.Generic;

namespace DiceSharp.Implementation.SyntaxTree
{
    internal abstract class FunctionDeclaration
    {
        public string Name { get; set; }
        public List<string> Arguments { get; set; }
    }
}