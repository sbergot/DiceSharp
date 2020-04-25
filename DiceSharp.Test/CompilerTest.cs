using System;
using System.Linq;
using Xunit;

using DiceSharp.Implementation;
using DiceSharp.Implementation.SyntaxTree;
using System.Collections.Generic;
using DiceSharp.Contracts;

namespace DiceSharp.Test
{
    public class CompilerTest
    {
        [Theory]
        [MemberData(nameof(Data))]
        internal void CompileSuccess(Ast ast, List<Roll> expectedResult)
        {
            var compiler = new Compiler();
            var program = compiler.Compile(ast);
            var diceRoller = new DiceRoller(1000, new Random(0));
            var result = program(diceRoller);
            Helpers.CompareObjects(expectedResult, result);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return TestData.GetTestData().Select(td => new object[] { td.Item2, td.Item3 });
            }
        }
    }
}
