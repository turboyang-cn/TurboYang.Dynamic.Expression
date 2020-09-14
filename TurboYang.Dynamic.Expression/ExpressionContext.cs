using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TurboYang.Dynamic.Expression
{
    public sealed class ExpressionContext
    {
        private Dictionary<String, Object> InternalVariables { get; } = new Dictionary<String, Object>();

        public ReadOnlyDictionary<String, Object> Variables
        {
            get
            {
                return new ReadOnlyDictionary<String, Object>(InternalVariables);
            }
        }

        public Object Lookup(String variableName)
        {
            InternalVariables.TryGetValue(variableName, out Object value);

            return value;
        }

        public T Lookup<T>(String variableName)
        {
            return (T)Convert.ChangeType(Lookup(variableName), typeof(T));
        }

        public Boolean TryLookup(String variableName, out Object value)
        {
            try
            {
                value = Lookup(variableName);

                return true;
            }
            catch
            {
                value = default;

                return false;
            }
        }

        public Boolean TryLookup<T>(String variableName, out T value)
        {
            try
            {
                value = Lookup<T>(variableName);

                return true;
            }
            catch
            {
                value = default;

                return false;
            }
        }

        public void Bind(String variableName, UInt64 value)
        {
            InternalVariables[variableName] = value;
        }

        public void Bind(String variableName, Decimal value)
        {
            InternalVariables[variableName] = value;
        }

        public void Bind(String variableName, Boolean value)
        {
            InternalVariables[variableName] = value;
        }

        public void Bind(String variableName, String value)
        {
            InternalVariables[variableName] = value;
        }
    }
}
