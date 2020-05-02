using System.Collections.Generic;
using DiceScript.Contracts;
using DiceScript.Implementation.SyntaxTree;

namespace DiceScript.Test
{
    internal class TestVector
    {
        public string Program { get; set; }
        public Script Script { get; set; }
        public List<Result> Results { get; set; }
    }
}