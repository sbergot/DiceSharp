using System.Collections.Generic;

namespace DiceScript.Implementation.SyntaxTree
{
    internal class RangeMappingStatement : Statement
    {
        public Scalar Scalar { get; set; }
        public List<RangeDeclaration> Ranges { get; set; }
    }
}