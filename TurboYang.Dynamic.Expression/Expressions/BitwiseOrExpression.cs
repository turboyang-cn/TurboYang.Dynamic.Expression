﻿using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public sealed class BitwiseOrExpression : BinaryExpression
    {
        public override String Pattern => @"^\|$";
        public sealed override Int32 Priority => 5;
        protected sealed override String Symbol => "|";

        public override Object Evaluate(ExpressionContext context)
        {
            OperandValue1 = Operand1.Evaluate(context);
            OperandValue2 = Operand2.Evaluate(context);

            return OperandValue1 | OperandValue2;
        }
    }
}
