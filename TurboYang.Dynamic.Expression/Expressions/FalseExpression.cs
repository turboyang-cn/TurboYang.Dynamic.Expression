using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public sealed class FalseExpression : BaseExpression
    {
        public override String Pattern => @"^false$";

        public FalseExpression()
        {
            IsBound = true;
        }

        public override Object Evaluate(ExpressionContext Context)
        {
            return false;
        }

        public override String VisualExpression => "false";

        public override String VisualSubstitutedExpression => VisualExpression;
    }
}
