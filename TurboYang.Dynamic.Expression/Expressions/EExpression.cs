using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public sealed class EExpression : ConstantExpression
    {
        public override String Pattern => @"^E$";

        public EExpression()
        {
            IsBound = true;
        }

        public override Object Evaluate(ExpressionContext Context)
        {
            return (Decimal)Math.E;
        }

        public override String VisualExpression => Math.E.ToString();

        public override String VisualSubstitutedExpression => VisualExpression;
    }
}
