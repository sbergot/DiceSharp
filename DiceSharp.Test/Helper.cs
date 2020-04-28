using System.Collections.Generic;
using System.Text.Json;
using DiceSharp.Contracts;
using DiceSharp.Implementation.SyntaxTree;
using Xunit;

namespace DiceSharp.Test
{
    internal static class Helpers
    {
        public static Script AssignStmt(string varName, DiceDeclaration exp)
        {
            return new Script
            {
                Statements = new List<Statement>
                {
                    new AssignementStatement {
                        VariableName = varName,
                        Expression = new DiceExpression { Dices = exp }
                    }
                }
            };
        }

        public static Script RangeMapStmt(Scalar scalar, List<RangeDeclaration> ranges)
        {
            return new Script
            {
                Statements = new List<Statement>
                {
                    new RangeMappingStatement {
                        Scalar = scalar,
                        Ranges = ranges
                    }
                }
            };
        }

        public static Script ToAst(DiceDeclaration exp)
        {
            return ToAst(new DiceExpression { Dices = exp });
        }

        public static Script ToAst(Expression exp)
        {
            var statement = new ExpressionStatement { Expression = exp };
            return new Script { Statements = new List<Statement> { statement } };
        }

        public static void CompareObjects(object a, object b)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new ConcreteTypeConverter<Statement>());
            options.Converters.Add(new ConcreteTypeConverter<Expression>());
            options.Converters.Add(new ConcreteTypeConverter<Option>());
            options.Converters.Add(new ConcreteTypeConverter<Result>());
            options.Converters.Add(new ConcreteTypeConverter<Scalar>());
            options.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            var sa = JsonSerializer.Serialize(a, options);
            var sb = JsonSerializer.Serialize(b, options);
            Assert.Equal(sa, sb);
        }
    }

}