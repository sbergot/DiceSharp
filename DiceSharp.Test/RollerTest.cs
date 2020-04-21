using System;
using System.Linq;
using Xunit;

using System.Collections.Generic;
using DiceSharp.Contracts;

namespace DiceSharp.Test
{
    public class RollerTest
    {
        [Theory]
        [MemberData(nameof(Data))]
        internal void CompileSuccess(string strProgram, List<Roll> expectedResult)
        {
            var roller = new Roller();
            roller.Random = new Random(0);
            var result = roller.Roll(strProgram);
            Helpers.CompareObjects(expectedResult, result);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return TestData.GetTestData().Select(td => new object[] { td.Item1, td.Item3 });
            }
        }
    }
}
