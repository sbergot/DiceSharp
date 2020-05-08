using System;
using Xunit;

using DiceScript.Contracts;
using DiceScript.Test.TestData;

namespace DiceScript.Test.TestCases
{
    public class BuilderTest
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

        [Theory]
        [ClassData(typeof(RangeMappingTestData))]
        internal void RangeMapRollCase(TestVector test)
        {
            RunSuccess(test);
        }

        [Theory]
        [ClassData(typeof(CalcTestData))]
        internal void CalcCase(TestVector test)
        {
            RunSuccess(test);
        }

        private static void RunSuccess(TestVector test)
        {
            var builder = new Builder(new Limitations { MaxRollNbr = 1000, MaxProgramSize = 1000 });
            builder.Random = new Random(0);
            var result = builder.BuildScript(test.Program)();
            Helpers.CompareObjects(test.Results, result);
        }
    }
}
