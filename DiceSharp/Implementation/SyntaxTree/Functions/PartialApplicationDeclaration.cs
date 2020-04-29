using System.Collections.Generic;

namespace DiceSharp.Implementation.SyntaxTree
{
    internal class PartialApplicationDeclaration : FunctionDeclaration
    {
        public string AppliedFunction { get; set; }
        public List<Scalar> ProvidedValues { get; set; }
    }
}