using System;
using System.Collections.Generic;
using DiceScript.Contracts;
using DiceScript.Implementation.SyntaxTree;

namespace DiceScript.Implementation
{
    internal class VariableContainer
    {
        private Dictionary<string, object> Variables { get; set; } = new Dictionary<string, object>();

        public VariableContainer()
        {
        }

        public VariableContainer(Dictionary<string, object> init)
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
                return GetVariableValue<int>(variable);
            }

            throw new InvalidOperationException($"Unknown scalar type: {scalar.GetType()}");
        }

        public T GetVariableValue<T>(VariableScalar variable)
        {
            if (!Variables.ContainsKey(variable.VariableName))
            {
                throw new InvalidScriptException($"Variable not found: {variable.VariableName}");
            }
            var value = Variables[variable.VariableName];
            if (value.GetType() != typeof(T))
            {
                throw new InvalidScriptException($"Incorrect type. Expected {typeof(T)} but got {value.GetType()}");
            }
            return (T)value;
        }

        public void SetVariable<T>(string name, T value)
        {
            if (Variables.ContainsKey(name))
            {
                throw new InvalidScriptException($"cannot overwrite variable: {name}");
            }
            Variables[name] = value;
        }
    }
}