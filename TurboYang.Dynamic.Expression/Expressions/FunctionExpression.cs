using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TurboYang.Dynamic.Expression.Expressions
{
    public abstract class FunctionExpression : BaseExpression
    {
        public abstract Int32 ParameterCount { get; }
        protected abstract String Name { get; }
        public sealed override Int32 Priority => 14;

        protected List<BaseExpression> InternalOperands { get; } = new List<BaseExpression>();

        protected List<dynamic> InternalOperandValues { get; } = new List<dynamic>();

        protected ReadOnlyCollection<BaseExpression> Operands
        {
            get
            {
                return new ReadOnlyCollection<BaseExpression>(InternalOperands);
            }
        }

        protected ReadOnlyCollection<dynamic> OperandValues
        {
            get
            {
                return new ReadOnlyCollection<dynamic>(InternalOperandValues);
            }
        }

        public void Bind(List<BaseExpression> Operands)
        {
            for (Int32 i = 0; i < ParameterCount; i++)
            {
                InternalOperands.Add(Operands[i]);
            }

            IsBound = true;
        }

        public override String VisualExpression
        {
            get
            {
                if (IsBound)
                {
                    return $"{Name}({String.Join(", ", InternalOperands.Select(x => x.VisualExpression))})";
                }
                else
                {
                    return $"{Name}()";
                }
            }
        }

        public override String VisualSubstitutedExpression
        {
            get
            {
                if (InternalOperandValues.Any(x => x == null))
                {
                    return String.Empty;
                }
                else
                {
                    return $"{Name}({String.Join(", ", InternalOperands.Select(x => x.VisualSubstitutedExpression))})";
                }
            }
        }
    }
}
