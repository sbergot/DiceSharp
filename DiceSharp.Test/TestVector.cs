using System.Collections.Generic;
using DiceSharp.Contracts;
using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Test
{
    internal class TestVector
    {
        public string Program { get; set; }
        public Script Script { get; set; }
        public List<Result> Results { get; set; }
    }
}