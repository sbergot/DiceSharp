using System.Collections.Generic;

namespace DiceSharp.Implementation.SyntaxTree
{
    internal class FunctionImplementation : FunctionDeclaration
    {
        public Script Script { get; set; }
    }
}