using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TurboYang.Dynamic.Expression.Tests.NetCore3
{
    [TestClass]
    public class ExpressionUnitTest
    {
        [TestMethod]
        public void FourFundamentalOperationTestMethod()
        {
            String expression = @"(1 + 2) * 3 / 4;";

            ExpressionCompiler compiler = new ExpressionCompiler();
            ExpressionContext context = new ExpressionContext();

            Assert.IsTrue(compiler.TryEvaluate(expression, context, out Decimal value) && value == (1 + 2) * 3 / 4);
        }

        [TestMethod]
        public void VariableTestMethod()
        {
            String expression = @"result1 = num1 + num2;" +
                                @"result2 = result1 * 10;" +
                                @"result2";

            ExpressionCompiler compiler = new ExpressionCompiler();
            ExpressionContext context = new ExpressionContext();

            Int32 num1 = 1;
            Int32 num2 = 2;

            context.Bind(nameof(num1), num1);
            context.Bind(nameof(num2), num2);

            Assert.IsTrue(compiler.TryEvaluate(expression, context, out Int32 value) && value == (num1 + num2) * 10
                && context.TryLookup("result1", out Int32 result1) && result1 == num1 + num2
                && context.TryLookup("result2", out Int32 result2) && result2 == value);
        }

        [TestMethod]
        public void LogicalOperationTestMethod()
        {
            String expression = @"(true || false) && true && 1 > 2;";

            ExpressionCompiler compiler = new ExpressionCompiler();
            ExpressionContext context = new ExpressionContext();

            Assert.IsTrue(compiler.TryEvaluate(expression, context, out Boolean value) && value == ((true || false) && true && 1 > 2));
        }

        [TestMethod]
        public void FunctionTestMethod()
        {
            String expression = @"Pow(2, 8);";

            ExpressionCompiler compiler = new ExpressionCompiler();
            ExpressionContext context = new ExpressionContext();

            Assert.IsTrue(compiler.TryEvaluate(expression, context, out Int32 value) && value == Math.Pow(2, 8));
        }

        [TestMethod]
        public void ConstantTestMethod()
        {
            String expression = @"PI * Pow(r, 2);";

            ExpressionCompiler compiler = new ExpressionCompiler();
            ExpressionContext context = new ExpressionContext();

            context.Bind("r", 10);

            Assert.IsTrue(compiler.TryEvaluate(expression, context, out Decimal value) && value == (Decimal)(Math.PI * Math.Pow(10, 2)));
        }
    }
}
