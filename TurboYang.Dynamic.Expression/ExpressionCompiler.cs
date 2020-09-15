using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using TurboYang.Dynamic.Expression.Expressions;

namespace TurboYang.Dynamic.Expression
{
    public sealed class ExpressionCompiler
    {
        private class Association
        {
            public Association(Type type, Regex regex, Int32 order)
            {
                Type = type;
                Regex = regex;
                Order = order;
            }

            public Regex Regex { get; }
            public Type Type { get; }
            public Int32 Order { get; } 
        }

        private List<Association> Associations { get; } = new List<Association>();

        public ExpressionCompiler()
        {
            RegisterExpression(Assembly.GetExecutingAssembly());
        }

        public void RegisterExpression(Type expressionType)
        {
            if (Activator.CreateInstance(expressionType) is BaseExpression expression)
            {
                Association association = new Association(expressionType, new Regex(expression.Pattern), expression.Order);

                Associations.Add(association);
            }
        }

        public void RegisterExpression(Assembly assembly)
        {
            foreach (Type expressionType in assembly.GetTypes().Where(Type => !Type.IsAbstract && Type.IsSubclassOf(typeof(BaseExpression)) && Type != typeof(NullExpression)))
            {
                RegisterExpression(expressionType);
            }
        }

        public Object Evaluate(String expression, ExpressionContext context)
        {
            return Compile(expression).Evaluate(context);
        }

        public T Evaluate<T>(String expression, ExpressionContext context)
        {
            return Compile(expression).Evaluate<T>(context);
        }

        public Boolean TryEvaluate(String expression, ExpressionContext context, out Object value)
        {
            try
            {
                value = Evaluate(expression, context);

                return true;
            }
            catch
            {
                value = null;

                return false;
            }
        }

        public Boolean TryEvaluate<T>(String expression, ExpressionContext context, out T value)
        {
            try
            {
                value = Evaluate<T>(expression, context);

                return true;
            }
            catch
            {
                value = default;

                return false;
            }
        }

        public ExpressionGroup Compile(String expressionString)
        {
            ExpressionGroup expressionGroup = new ExpressionGroup();

            try
            {
                List<List<BaseExpression>> expressionGroups = CreateExpressionGroups(expressionString);

                foreach (List<BaseExpression> expressions in expressionGroups)
                {
                    Stack<BaseExpression> expressionStack = SerializationExpressions(expressions);

                    if (expressionStack.Count > 0)
                    {
                        expressionGroup.Add(BuildExpressionTree(expressionStack));
                    }
                }
            }
            catch (Exception exception)
            {
                throw new ArgumentException("Is not a valid expression.", exception);
            }

            return expressionGroup;
        }

        private List<List<BaseExpression>> CreateExpressionGroups(String expressionString)
        {
            List<BaseExpression> expressions = Tokenize(expressionString);

            return CompressExpression(expressions);
        }

        private List<BaseExpression> Tokenize(String expressionString)
        {
            List<BaseExpression> expressions = new List<BaseExpression>();

            for (Int32 i = 0; i < expressionString.Length; i++)
            {
                Boolean isMatched = false;
                String effectiveToken = String.Empty;
                for (Int32 j = expressionString.Length - i; j > 0 && !isMatched; j--)
                {
                    String token = expressionString.Substring(i, j).Trim();

                    if (effectiveToken.Length != token.Length)
                    {
                        effectiveToken = token;

                        foreach (Association association in Associations.OrderByDescending(x => x.Order))
                        {
                            if (association.Regex.IsMatch(token))
                            {
                                isMatched = true;
                                expressions.Add(CreateExpression(token, association.Type));
                                i += j - 1;
                                break;
                            }
                        }
                    }
                }

                if (!isMatched)
                {
                    return new List<BaseExpression>();
                }
            }

            return expressions;
        }

        private BaseExpression CreateExpression(String token, Type expressionType)
        {
            BaseExpression expression = Activator.CreateInstance(expressionType) as BaseExpression;

            if (expression is NumericExpression numericExpression)
            {
                numericExpression.Bind(token);
            }
            else if (expression is VariableExpression variableExpression)
            {
                variableExpression.Bind(token);
            }
            else if (expression is StringExpression stringExpression)
            {
                stringExpression.Bind(token);
            }

            return expression;
        }

        private List<List<BaseExpression>> CompressExpression(List<BaseExpression> expressions)
        {
            List<List<BaseExpression>> expressionGroups = new List<List<BaseExpression>>();
            List<BaseExpression> expressionGroup = new List<BaseExpression>();

            for (Int32 i = 0; i < expressions.Count - 1; i++)
            {
                BaseExpression currentExpression = expressions[i];

                if (currentExpression is BreakExpression)
                {
                    expressionGroups.Add(new List<BaseExpression>(expressionGroup));

                    expressionGroup.Clear();
                }
                else
                {
                    BaseExpression previousExpression = new NullExpression();
                    BaseExpression nextExpression = new NullExpression();

                    if (i - 1 >= 0)
                    {
                        previousExpression = expressions[i - 1];
                    }
                    if (i + 1 < expressions.Count)
                    {
                        nextExpression = expressions[i + 1];
                    }

                    if ((currentExpression is AdditionExpression || currentExpression is SubtractionExpression) && !(previousExpression is StringExpression || previousExpression is NumericExpression || previousExpression is VariableExpression || previousExpression is RightParentheseExpression) && (nextExpression is NumericExpression || nextExpression is VariableExpression))
                    {
                        NumericExpression zeroExpression = new NumericExpression();
                        zeroExpression.Bind("0");

                        expressionGroup.Add(new LeftParentheseExpression());
                        expressionGroup.Add(zeroExpression);
                        expressionGroup.Add(currentExpression);
                        expressionGroup.Add(nextExpression);
                        expressionGroup.Add(new RightParentheseExpression());

                        i += 1;
                    }
                    else
                    {
                        expressionGroup.Add(currentExpression);
                    }
                }
            }

            if (expressions.Count > 0)
            {
                BaseExpression currentExpression = expressions[expressions.Count - 1];

                if (!(currentExpression is BreakExpression))
                {
                    expressionGroup.Add(currentExpression);
                }
            }

            expressionGroups.Add(expressionGroup);

            return expressionGroups.Where(x => x.Count > 0).ToList();
        }

        private Stack<BaseExpression> SerializationExpressions(List<BaseExpression> expressions)
        {
            Stack<BaseExpression> operandStack = new Stack<BaseExpression>();
            Stack<BaseExpression> operatorStack = new Stack<BaseExpression>();
            Stack<BaseExpression> expressionStack = new Stack<BaseExpression>();

            operatorStack.Push(new NullExpression());

            foreach (BaseExpression expression in expressions)
            {
                if (expression.Priority == 0)
                {
                    operandStack.Push(expression);
                }
                else
                {
                    if (expression is LeftParentheseExpression)
                    {
                        operatorStack.Push(expression);
                    }
                    else if (expression is RightParentheseExpression)
                    {
                        while (!(operatorStack.Peek() is NullExpression))
                        {
                            BaseExpression operatorExpression = operatorStack.Pop();

                            if (operatorExpression is LeftParentheseExpression)
                            {
                                if (operatorStack.Peek() is FunctionExpression)
                                {
                                    operandStack.Push(operatorStack.Pop());
                                }

                                break;
                            }
                            else
                            {
                                operandStack.Push(operatorExpression);
                            }
                        }
                    }
                    else if (expression is CommaExpression)
                    {
                        while (!(operatorStack.Peek() is NullExpression))
                        {
                            BaseExpression operatorExpression = operatorStack.Pop();

                            if (operatorExpression is LeftParentheseExpression)
                            {
                                operatorStack.Push(operatorExpression);

                                break;
                            }
                            else
                            {
                                operandStack.Push(operatorExpression);
                            }
                        }
                    }
                    else
                    {
                        if (operatorStack.Peek() is LeftParentheseExpression || operatorStack.Peek() is NullExpression)
                        {
                            operatorStack.Push(expression);
                        }
                        else if (expression.Priority > operatorStack.Peek().Priority)
                        {
                            operatorStack.Push(expression);
                        }
                        else
                        {
                            while (!(operatorStack.Peek() is NullExpression))
                            {
                                if (expression.Priority > operatorStack.Peek().Priority || operatorStack.Peek() is LeftParentheseExpression)
                                {
                                    operatorStack.Push(expression);
                                    break;
                                }
                                else
                                {
                                    operandStack.Push(operatorStack.Pop());

                                    if (operatorStack.Peek() is NullExpression)
                                    {
                                        operatorStack.Push(expression);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            while (!(operatorStack.Peek() is NullExpression))
            {
                operandStack.Push(operatorStack.Pop());
            }

            while (operandStack.Count > 0)
            {
                expressionStack.Push(operandStack.Pop());
            }

            return expressionStack;
        }

        private BaseExpression BuildExpressionTree(Stack<BaseExpression> expressionStack)
        {
            expressionStack = new Stack<BaseExpression>(expressionStack.Reverse());
            Stack<BaseExpression> operandStack = new Stack<BaseExpression>();

            while (expressionStack.Count > 0)
            {
                BaseExpression expression = expressionStack.Pop();

                if (expression.Priority == 0)
                {
                    operandStack.Push(expression);
                }
                else
                {
                    if (expression is BinaryExpression binaryExpression)
                    {
                        BaseExpression Operand1 = operandStack.Pop();
                        BaseExpression Operand2 = operandStack.Pop();

                        binaryExpression.Bind(Operand2, Operand1);

                        operandStack.Push(binaryExpression);
                    }
                    else if (expression is FunctionExpression functionExpression)
                    {
                        List<BaseExpression> operands = new List<BaseExpression>();

                        for (Int32 i = 0; i < functionExpression.ParameterCount; i++)
                        {
                            operands.Add(operandStack.Pop());
                        }

                        operands.Reverse();

                        functionExpression.Bind(operands);

                        operandStack.Push(functionExpression);
                    }
                }
            }

            return operandStack.Pop();
        }
    }
}
