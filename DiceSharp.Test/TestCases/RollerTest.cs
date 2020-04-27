using System;
using Xunit;

using DiceSharp.Contracts;
using DiceSharp.Test.TestData;

namespace DiceSharp.Test.TestCases
{
    public class RunnerTest
    {
        [Theory]
        [ClassData(typeof(BasicTestData))]
        internal void BasicRollCase(TestVector test)
        {
            RunSuccess(test);
        }

        [Theory]
        [ClassData(typeof(AssignTestData))]
        internal void AssignRollCase(TestVector test)
        {
            RunSuccess(test);
        }

        [Theory]
        [ClassData(typeof(RichDiceTestData))]
        internal void RichDiceRollCase(TestVector test)
        {
            RunSuccess(test);
        }

        private static void RunSuccess(TestVector test)
        {
            var roller = new Runner(new Limitations { MaxRollNbr = 1000, MaxProgramSize = 1000 });
            roller.Random = new Random(0);
            var result = roller.Roll(test.Program);
            Helpers.CompareObjects(test.Results, result);
        }
    }
}
