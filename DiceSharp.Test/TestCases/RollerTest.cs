using System;
using Xunit;

using DiceSharp.Contracts;
using DiceSharp.Test.TestData;

namespace DiceSharp.Test.TestCases
{
    public class RollerTest
    {
        [Theory]
        [ClassData(typeof(BasicTestData))]
        internal void BasicRollCase(TestVector test)
        {
            RollSuccess(test);
        }

        [Theory]
        [ClassData(typeof(RichDiceTestData))]
        internal void RichDiceRollCase(TestVector test)
        {
            RollSuccess(test);
        }

        private static void RollSuccess(TestVector test)
        {
            var roller = new Roller(new Limitations { MaxRollNbr = 1000, MaxProgramSize = 1000 });
            roller.Random = new Random(0);
            var result = roller.Roll(test.Program);
            Helpers.CompareObjects(test.Results, result);
        }
    }
}
