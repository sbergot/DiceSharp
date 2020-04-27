using System.Collections.Generic;

namespace DiceSharp.Implementation.SyntaxTree
{
    internal class RangeMappingStatement : Statement
    {
        public Scalar Scalar { get; set; }
        public List<RangeDeclaration> Ranges { get; set; }
    }
}