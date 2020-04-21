using Xunit;

using DiceSharp.Implementation;
using DiceSharp.Implementation.SyntaxTree;
using System.Collections.Generic;
using System.Linq;

namespace DiceSharp.Test
{
    public class ParserTest
    {
        [Theory]
        [MemberData(nameof(Data))]
        internal void ParsingSuccess(string program, Ast expectedAst)
        {
            var parser = new Parser();
            var ast = parser.Parse(program);
            Helpers.CompareObjects(expectedAst, ast);
        }
        public static IEnumerable<object[]> Data
        {
            get
            {
                return TestData.GetTestData().Select(td => new object[] { td.Item1, td.Item2 });
            }
        }
    }
}
