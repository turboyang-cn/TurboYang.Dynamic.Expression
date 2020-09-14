using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public abstract class BaseExpression
    {
        public Boolean IsBound { get; protected set; }

        public abstract String Pattern { get; }
        public abstract String VisualExpression { get; }
        public abstract String VisualSubstitutedExpression { get; }
        public virtual Int32 Priority { get; } = 0;

        public abstract Object Evaluate(ExpressionContext context);

        public override String ToString()
        {
            return VisualExpression;
        }
    }
}
