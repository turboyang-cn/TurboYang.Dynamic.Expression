using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public sealed class VariableExpression : BaseExpression
    {
        public override String Pattern => @"^\w+$";
        public String VariableName { get; private set; }
        public dynamic VariableValue { get; private set; }

        public void Bind(String variableName)
        {
            VariableName = variableName;

            IsBound = true;
        }

        public override Object Evaluate(ExpressionContext context)
        {
            VariableValue = context.Lookup(VariableName);

            return VariableValue;
        }

        public override String VisualExpression => VariableName ?? null;

        public override String VisualSubstitutedExpression => VariableValue?.ToString() ?? null;
    }
}
