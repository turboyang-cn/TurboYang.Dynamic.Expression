# TurboYang.Dynamic.Expression
[![Build Status](https://vsrm.dev.azure.com/TurboYang-CN/_apis/public/Release/badge/b1f4b2d3-adc7-4170-8228-4d7cb73777cc/1/1)](https://vsrm.dev.azure.com/TurboYang-CN/_apis/public/Release/badge/b1f4b2d3-adc7-4170-8228-4d7cb73777cc/1/1) [![NuGet version (TurboYang.Dynamic.Expression)](https://img.shields.io/nuget/v/TurboYang.Dynamic.Expression.svg?style=flat)](https://www.nuget.org/packages/TurboYang.Dynamic.Expression/) [![MIT License](https://img.shields.io/badge/license-MIT-green.svg)](https://github.com/turboyang-cn/TurboYang.Dynamic.Expression/blob/master/LICENSE)

 The library for compile and evaluate custom expressions. Implementation for .NET Standard 2.0.

## Installation
Install with Package Manager:
```
Install-Package TurboYang.Dynamic.Expression
```

## Usage: A simple expression
``` CSharp
String expression = @"(1 + 2) * 3 / 3;";

ExpressionCompiler compiler = new ExpressionCompiler();
ExpressionContext context = new ExpressionContext();

Decimal result = compiler.Evaluate<Decimal>(expression, context);      // result == 3
```

## Usage: Expression with parameters
``` CSharp
String expression = @"(x + y) * z;";

ExpressionCompiler compiler = new ExpressionCompiler();
ExpressionContext context = new ExpressionContext();

context.Bind("x", 1);
context.Bind("y", 2);
context.Bind("z", 3);

Int32 result = compiler.Evaluate<Int32>(expression, context);      // result == 9
```
You can also bind parameters through anonymous class.
``` CSharp
String expression = @"(x + y) * z;";

ExpressionCompiler compiler = new ExpressionCompiler();
ExpressionContext context = new ExpressionContext(new {
    x = 1,
    y = 2,
    z = 3
});

Int32 result = compiler.Evaluate<Int32>(expression, context);      // result == 9
```

## Usage: Function expression
``` CSharp
String expression = @"Pow(2, 3);";

ExpressionCompiler compiler = new ExpressionCompiler();
ExpressionContext context = new ExpressionContext();

Decimal result = compiler.Evaluate<Decimal>(expression, context);      // result == 8
```

## Usage: Constant expression
``` CSharp
String expression = @"PI * Pow(r, 2);";

ExpressionCompiler compiler = new ExpressionCompiler();
ExpressionContext context = new ExpressionContext(new {
    r = 10;
});

Decimal result = compiler.Evaluate<Decimal>(expression, context);      // result == 314.159265358979
```

## Usage: Intervening variable
``` CSharp
String expression = @"x = a + b; y = c + d; x * y";

ExpressionCompiler compiler = new ExpressionCompiler();
ExpressionContext context = new ExpressionContext(new {
    a = 1,
    b = 2,
    c = 3,
    d = 4
});

Int32 result = compiler.Evaluate<Int32>(expression, context);      // result == 21
```
You can call `ExpressionContext.Lookup` Method to get the intervening variable value.
``` CSharp
Int32 x = context.Lookup<Int32>("x");         // 3
Int32 y = context.Lookup<Int32>("y");         // 7
```

## Usage: Custom function
You can customize your functions, such as defining a function(`Circumference`) to calculate the circumference of a circle.
``` CSharp
public class CircumferenceExpression : FunctionExpression
{
    public override Int32 ParameterCount => 1;

    protected override String Name => "Circumference";

    public override Object Evaluate(ExpressionContext context)
    {
        // Please do not remove this code
        base.Evaluate(context);

        return 2 * (Decimal)Math.PI * OperandValues[0];
    }
}
```
Then register and evaluate your function expression.
``` CSharp
String expression = @"Circumference(r)";

ExpressionCompiler compiler = new ExpressionCompiler();
compiler.RegisterExpression(typeof(CircumferenceExpression));

ExpressionContext context = new ExpressionContext(new
{
    r = 1
});

Decimal result = compiler.Evaluate<Decimal>(expression, context);      // result == 6.28318530717958
```

Built in expression
| Expression | Symbol | Example |
| ---- | ---------- | --------- |
| Addition | + | x + y |
| Subtraction | - | x - y |
| Multiply | * | x * y |
| Division | / | x / y |
| Modulus | % | x % y |
| Left Shift | << | x << y |
| Right Shift | >> | x >> y |
| Left Parenthese | ( | (x + y) * z |
| Right Parenthese | ) | (x + y) * z |
| Bitwise And | & | x & y |
| Bitwise Or | \| | x \| y |
| Exclusive Or | ^ | x ^ y |
| And | && | x && y |
| Or | \|\| | x \|\| y |
| Break | ; | x + y; |
| Comma | , | Pow(x, y) |
| True | true | x == true |
| False | false | x == false |
| Equality | == | x == y |
| Inequality | != | x != y |
| Less Than | < | x < y |
| Less Than Or Equal | <= | x <= y |
| Greater Than | > | x > y |
| Greater Than Or Equal | >= | x >= y |
| Pow | Pow() | Pow(x, y) |
| Numeric |  | 3.14 |
| String |  | "Hello world!" |
| Variable |  | x |
| PI Ratio | PI | PI |
| Euler's Number | E | E |
