using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public sealed class NullExpression : BaseExpression
    {
        public override String Pattern => @"^$";
        public sealed override Int32 Priority => Int32.MaxValue;

        public NullExpression()
        {
            IsBound = true;
        }

        public override Object Evaluate(ExpressionContext context)
        {
            return null;
        }

        public override String VisualExpression => String.Empty;

        public override String VisualSubstitutedExpression => VisualExpression;
    }
}
