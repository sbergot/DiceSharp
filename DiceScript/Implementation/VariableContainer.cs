using System;
using System.Collections.Generic;
using DiceScript.Contracts;
using DiceScript.Implementation.SyntaxTree;

namespace DiceScript.Implementation
{
    internal class VariableContainer
    {
        private Dictionary<string, int> Variables { get; set; } = new Dictionary<string, int>();

        public VariableContainer()
        {
        }

        public VariableContainer(Dictionary<string, int> init)
        {
            Variables = init;
        }

        public int GetScalarValue(Scalar scalar)
        {
            _ = scalar ?? throw new ArgumentNullException(nameof(scalar));

            if (scalar is ConstantScalar constant)
            {
                return constant.Value;
            }

            if (scalar is VariableScalar variable)
            {
                if (!Variables.ContainsKey(variable.VariableName))
                {
                    throw new InvalidScriptException($"Variable not found: {variable.VariableName}");
                }
                return Variables[variable.VariableName];
            }

            throw new InvalidOperationException($"Unknown scalar type: {scalar.GetType()}");
        }

        public void SetVariable(string name, int value)
        {
            if (Variables.ContainsKey(name))
            {
                throw new InvalidScriptException($"cannot overwrite variable: {name}");
            }
            Variables[name] = value;
        }
    }
}