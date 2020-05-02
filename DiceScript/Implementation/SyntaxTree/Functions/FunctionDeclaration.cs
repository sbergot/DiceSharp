using System.Collections.Generic;

namespace DiceScript.Implementation.SyntaxTree
{
    internal abstract class FunctionDeclaration
    {
        public string Name { get; set; }
        public List<string> Arguments { get; set; }
    }
}