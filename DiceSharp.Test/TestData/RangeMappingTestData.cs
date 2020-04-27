using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiceSharp.Contracts;
using DiceSharp.Implementation;
using DiceSharp.Implementation.SyntaxTree;

namespace DiceSharp.Test.TestData
{
    internal class RangeMappingTestData : IEnumerable<object[]>
    {
        public static List<TestVector> GetTestData()
        {
            return new List<(string, Ast, List<Result>)>
            {
            (
                "range 4 ((\"hello rangemap\";default))",
                Helpers.RangeMapStmt(
                    new ConstantScalar { Value = 4 },
                    new List<RangeDeclaration>
                    {
                        new RangeDeclaration
                        {
                            Value = "hello rangemap",
                            Filter = new FilterOption { Type = FilterType.None }
                        }
                    }),
                new List<Result> {
                    new PrintResult
                    {
                        Value = "hello rangemap",
                    }
                }
            ),
            }
            .Select(t => new TestVector { Program = t.Item1, Ast = t.Item2, Results = t.Item3 })
            .ToList();
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            return GetTestData()
                .Select(t => new object[] { t })
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetTestData()
                .Select(t => new object[] { t })
                .GetEnumerator();
        }
    }

}