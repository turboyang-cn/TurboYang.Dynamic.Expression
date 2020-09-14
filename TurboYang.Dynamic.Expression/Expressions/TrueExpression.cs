using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public sealed class TrueExpression : BaseExpression
    {
        public override String Pattern => @"^true$";

        public TrueExpression()
        {
            IsBound = true;
        }

        public override Object Evaluate(ExpressionContext Context)
        {
            return true;
        }

        public override String VisualExpression => "true";

        public override String VisualSubstitutedExpression => VisualExpression;
    }
}
