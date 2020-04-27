using System.Collections.Generic;
using System.Text.Json;
using DiceSharp.Contracts;
using DiceSharp.Implementation.SyntaxTree;
using Xunit;

namespace DiceSharp.Test
{
    internal static class Helpers
    {
        public static Ast AssignStmt(string varName, DiceDeclaration exp)
        {
            return new Ast
            {
                Statements = new List<Statement>
                {
                    new AssignementStatement {
                        VariableName = varName,
                        Expression = new RichDiceExpression { Dices = exp }
                    }
                }
            };
        }

        public static Ast ToAst(DiceDeclaration exp)
        {
            return ToAst(new RichDiceExpression { Dices = exp });
        }

        public static Ast ToAst(Expression exp)
        {
            var statement = new ExpressionStatement { Expression = exp };
            return new Ast { Statements = new List<Statement> { statement } };
        }

        public static void CompareObjects(object a, object b)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new ConcreteTypeConverter<Statement>());
            options.Converters.Add(new ConcreteTypeConverter<Expression>());
            options.Converters.Add(new ConcreteTypeConverter<Option>());
            options.Converters.Add(new ConcreteTypeConverter<Result>());
            options.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            var sa = JsonSerializer.Serialize(a, options);
            var sb = JsonSerializer.Serialize(b, options);
            Assert.Equal(sa, sb);
        }
    }

}