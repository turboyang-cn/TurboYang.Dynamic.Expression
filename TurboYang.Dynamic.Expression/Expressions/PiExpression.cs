using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public sealed class PiExpression : ConstantExpression
    {
        public override String Pattern => @"^PI$";

        public PiExpression()
        {
            IsBound = true;
        }

        public override Object Evaluate(ExpressionContext Context)
        {
            return Math.PI;
        }

        public override String VisualExpression => Math.PI.ToString();

        public override String VisualSubstitutedExpression => VisualExpression;
    }
}
