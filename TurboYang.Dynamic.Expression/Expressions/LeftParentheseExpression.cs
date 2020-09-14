using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public sealed class LeftParentheseExpression : BaseExpression
    {
        public override String Pattern => @"^\($";
        public sealed override Int32 Priority => 14;

        public LeftParentheseExpression()
        {
            IsBound = true;
        }

        public override Object Evaluate(ExpressionContext context)
        {
            return null;
        }

        public override String VisualExpression => "(";

        public override String VisualSubstitutedExpression => "(";
    }
}
