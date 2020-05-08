using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DiceScript.Contracts;
using DiceScript.Implementation.SyntaxTree;
using Xunit;

namespace DiceScript.Test
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

        public static Script ToAst(params Expression[] exps)
        {
            var statements = exps
                .Select(exp => new ExpressionStatement { Expression = exp })
                .Cast<Statement>()
                .ToList();
            return new Script { Statements = statements };
        }

        public static void CompareObjects(object a, object b)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new ConcreteTypeConverter<Statement>());
            options.Converters.Add(new ConcreteTypeConverter<Expression>());
            options.Converters.Add(new ConcreteTypeConverter<Option>());
            options.Converters.Add(new ConcreteTypeConverter<Result>());
            options.Converters.Add(new ConcreteTypeConverter<Scalar>());
            options.Converters.Add(new ConcreteTypeConverter<FunctionDeclaration>());
            options.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            var sa = JsonSerializer.Serialize(a, options);
            var sb = JsonSerializer.Serialize(b, options);
            Assert.Equal(sa, sb);
        }
    }

}