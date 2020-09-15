using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public sealed class PowExpression : FunctionExpression
    {
        public override Int32 ParameterCount => 2;
        protected override String Name => "Pow";

        public override Object Evaluate(ExpressionContext context)
        {
            base.Evaluate(context);

            return Math.Pow(OperandValues[0], OperandValues[1]);
        }
    }
}
