using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public sealed class NumericExpression : BaseExpression
    {
        public override String Pattern => @"^\d+(\.\d+)?$";
        public Object Value { get; private set; }

        public void Bind(String value)
        {
            if (value.Contains("."))
            {
                Value = Decimal.Parse(value);
            }
            else
            {
                Value = UInt64.Parse(value);
            }

            IsBound = true;
        }

        public override Object Evaluate(ExpressionContext context)
        {
            return Value;
        }

        public override String VisualExpression => Value?.ToString() ?? "null";

        public override String VisualSubstitutedExpression => VisualExpression;
    }
}
