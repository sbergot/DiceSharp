using Xunit;

using DiceScript.Implementation.Parsing;
using DiceScript.Test.TestData;

namespace DiceScript.Test.TestCases
{
    public class ParserTest
    {
        [Theory]
        [ClassData(typeof(BasicTestData))]
        internal void BasicParseCase(TestVector test)
        {
            ParsingSuccess(test);
        }

        [Theory]
        [ClassData(typeof(AssignTestData))]
        internal void AssignParseCase(TestVector test)
        {
            ParsingSuccess(test);
        }

        [Theory]
        [ClassData(typeof(RichDiceTestData))]
        internal void RichDiceParseCase(TestVector test)
        {
            ParsingSuccess(test);
        }

        [Theory]
        [ClassData(typeof(RangeMappingTestData))]
        internal void RangeMapParseCase(TestVector test)
        {
            ParsingSuccess(test);
        }

        [Theory]
        [ClassData(typeof(CalcTestData))]
        internal void CalcCase(TestVector test)
        {
            ParsingSuccess(test);
        }

        [Theory]
        [ClassData(typeof(AggregationTestData))]
        internal void DiceVarCase(TestVector test)
        {
            ParsingSuccess(test);
        }

        private static void ParsingSuccess(TestVector test)
        {
            var parser = new Parser();
            var ast = parser.ParseScript(test.Program);
            Helpers.CompareObjects(test.Script, ast);
        }
    }
}
