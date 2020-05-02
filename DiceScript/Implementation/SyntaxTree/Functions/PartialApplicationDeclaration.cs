using System.Collections.Generic;

namespace DiceScript.Implementation.SyntaxTree
{
    internal class PartialApplicationDeclaration : FunctionDeclaration
    {
        public string AppliedFunction { get; set; }
        public List<Scalar> ProvidedValues { get; set; }
    }
}