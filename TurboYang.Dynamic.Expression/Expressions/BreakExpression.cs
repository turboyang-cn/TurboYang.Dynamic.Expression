using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public sealed class BreakExpression : BaseExpression
    {
        public override String Pattern => @"^;$";

        public BreakExpression()
        {
            IsBound = true;
        }

        public override Object Evaluate(ExpressionContext context)
        {
            return null;
        }

        public override String VisualExpression => ";";

        public override String VisualSubstitutedExpression => VisualExpression;
    }
}
