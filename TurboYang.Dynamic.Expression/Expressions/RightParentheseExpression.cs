using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public sealed class RightParentheseExpression : BaseExpression
    {
        public override String Pattern => @"^\)$";
        public sealed override Int32 Priority => 14;

        public RightParentheseExpression()
        {
            IsBound = true;
        }

        public override Object Evaluate(ExpressionContext Context)
        {
            return null;
        }

        public override String VisualExpression => ")";

        public override String VisualSubstitutedExpression => ")";
    }
}
