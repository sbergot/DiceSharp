using System.Collections.Generic;

namespace DiceScript.Implementation.SyntaxTree
{
    internal class FunctionImplementation : FunctionDeclaration
    {
        public Script Script { get; set; }
    }
}