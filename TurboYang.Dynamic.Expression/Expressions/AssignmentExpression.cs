using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public sealed class AssignmentExpression : BinaryExpression
    {
        public override String Pattern => @"^=$";
        public sealed override Int32 Priority => 1;
        protected sealed override String Symbol => "=";

        public override Object Evaluate(ExpressionContext context)
        {
            if (Operand1 is VariableExpression variableExpression)
            {
                OperandValue2 = Operand2.Evaluate(context);

                context.Bind(variableExpression.VariableName, OperandValue2);
            }

            return null;
        }

        public override String VisualExpression
        {
            get
            {
                if (IsBound)
                {
                    return $"{Operand1.VisualExpression} {Symbol} {Operand2.VisualExpression}";
                }
                else
                {
                    return Symbol;
                }
            }
        }

        public override String VisualSubstitutedExpression
        {
            get
            {
                if (OperandValue2 == null)
                {
                    return String.Empty;
                }
                else
                {
                    return $"{Operand1.VisualSubstitutedExpression} {Symbol} {Operand2.VisualSubstitutedExpression}";
                }
            }
        }
    }
}
