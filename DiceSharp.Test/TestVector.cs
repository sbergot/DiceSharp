using System.Collections.Generic;
using DiceSharp.Contracts;
using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Test
{
    internal class TestVector
    {
        public string Program { get; set; }
        public Ast Ast { get; set; }
        public List<Result> Results { get; set; }
    }
}