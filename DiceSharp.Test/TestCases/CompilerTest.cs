using System;
using Xunit;

using DiceSharp.Implementation;
using DiceSharp.Test.TestData;

namespace DiceSharp.Test.TestCases
{
    public class CompilerTest
    {
        [Theory]
        [ClassData(typeof(BasicTestData))]
        internal void BasicCompileCase(TestVector test)
        {
            CompileSuccess(test);
        }

        [Theory]
        [ClassData(typeof(RichDiceTestData))]
        internal void RichDiceCompileCase(TestVector test)
        {
            CompileSuccess(test);
        }

        internal void CompileSuccess(TestVector test)
        {
            var compiler = new Compiler();
            var program = compiler.Compile(test.Ast);
            var diceRoller = new DiceRoller(1000, new Random(0));
            var result = program(diceRoller);
            Helpers.CompareObjects(test.Results, result);
        }
    }
}
