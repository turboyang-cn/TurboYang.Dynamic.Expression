using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public sealed class OrExpression : BinaryExpression
    {
        public override String Pattern => @"^\|\|$";
        public sealed override Int32 Priority => 3;
        protected sealed override String Symbol => "||";

        public override Object Evaluate(ExpressionContext context)
        {
            OperandValue1 = Operand1.Evaluate(context);
            OperandValue2 = Operand2.Evaluate(context);

            return OperandValue1 || OperandValue2;
        }
    }
}
