using System;
using System.Collections.Generic;
using TurboYang.Dynamic.Expression.Expressions;

namespace TurboYang.Dynamic.Expression
{
    public sealed class ExpressionGroup : List<BaseExpression>
    {
        public Object Evaluate(ExpressionContext context)
        {
            try
            {
                Object result = null;

                foreach (BaseExpression expression in this)
                {
                    result = expression.Evaluate(context);
                }

                return result;
            }
            catch (Exception exception)
            {
                throw new ArgumentException("Unable to calculate the result.", exception);
            }
        }

        public T Evaluate<T>(ExpressionContext context)
        {
            try
            {
                return (T)Convert.ChangeType(Evaluate(context), typeof(T));
            }
            catch (Exception exception)
            {
                throw new ArgumentException("Unable to calculate the result.", exception);
            }
        }

        public Boolean TryEvaluate(ExpressionContext context, out Object value)
        {
            try
            {
                value = Evaluate(context);

                return true;
            }
            catch
            {
                value = null;

                return false;
            }
        }

        public Boolean TryEvaluate<T>(ExpressionContext context, out T value)
        {
            try
            {
                value = Evaluate<T>(context);

                return true;
            }
            catch
            {
                value = default;

                return false;
            }
        }
    }
}
