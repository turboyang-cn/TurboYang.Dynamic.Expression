using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public sealed class PowExpression : FunctionExpression
    {
        public override String Pattern => @"^Pow$";
        public override Int32 ParameterCount => 2;
        protected override String Name => "Pow";

        public override Object Evaluate(ExpressionContext context)
        {
            for (Int32 i = 0; i < ParameterCount; i++)
            {
                InternalOperandValues.Add(InternalOperands[i].Evaluate(context));
            }

            return Math.Pow(InternalOperandValues[0], InternalOperandValues[1]);
        }
    }
}
