using System;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public abstract class BinaryExpression : BaseExpression
    {
        protected abstract String Symbol { get; }
        public BaseExpression Operand1 { get; private set; }
        public BaseExpression Operand2 { get; private set; }
        public dynamic OperandValue1 { get; protected set; }
        public dynamic OperandValue2 { get; protected set; }

        public void Bind(BaseExpression operand1, BaseExpression operand2)
        {
            Operand1 = operand1;
            Operand2 = operand2;

            IsBound = true;
        }

        public override String VisualExpression
        {
            get
            {
                if (IsBound)
                {
                    return $"({Operand1.VisualExpression} {Symbol} {Operand2.VisualExpression})";
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
                if (OperandValue1 == null || OperandValue2 == null)
                {
                    return String.Empty;
                }
                else
                {
                    return $"({Operand1.VisualSubstitutedExpression} {Symbol} {Operand2.VisualSubstitutedExpression})";
                }
            }
        }
    }
}
