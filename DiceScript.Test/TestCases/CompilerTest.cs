using System;
using Xunit;

using DiceScript.Implementation;
using DiceScript.Test.TestData;

namespace DiceScript.Test.TestCases
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
        [ClassData(typeof(AssignTestData))]
        internal void AssignCompileCase(TestVector test)
        {
            CompileSuccess(test);
        }

        [Theory]
        [ClassData(typeof(RichDiceTestData))]
        internal void RichDiceCompileCase(TestVector test)
        {
            CompileSuccess(test);
        }

        [Theory]
        [ClassData(typeof(RangeMappingTestData))]
        internal void RangeMapCompileCase(TestVector test)
        {
            CompileSuccess(test);
        }

        internal void CompileSuccess(TestVector test)
        {
            var compiler = new Compiler();
            var program = compiler.CompileScript(test.Script);
            var diceRoller = new DiceRoller(1000, new Random(0));
            var result = program(diceRoller);
            Helpers.CompareObjects(test.Results, result);
        }
    }
}
