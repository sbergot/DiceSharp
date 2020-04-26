using Xunit;

using DiceSharp.Implementation.Parsing;
using DiceSharp.Test.TestData;

namespace DiceSharp.Test.TestCases
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

        private static void ParsingSuccess(TestVector test)
        {
            var parser = new Parser();
            var ast = parser.Parse(test.Program);
            Helpers.CompareObjects(test.Ast, ast);
        }
    }
}
