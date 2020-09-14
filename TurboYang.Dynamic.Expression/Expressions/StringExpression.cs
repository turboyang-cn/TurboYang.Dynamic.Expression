using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public sealed class StringExpression : BaseExpression
    {
        public override String Pattern => @"^""((\\"")+|[^\f\r\n\v""]+)+""$";
        public String Value { get; private set; }

        public void Bind(String value)
        {
            Value = value.Substring(1, value.Length - 2);

            IsBound = true;
        }

        public override Object Evaluate(ExpressionContext context)
        {
            return Value;
        }

        public override String VisualExpression => Value ?? "null";

        public override String VisualSubstitutedExpression => VisualExpression;
    }
}
