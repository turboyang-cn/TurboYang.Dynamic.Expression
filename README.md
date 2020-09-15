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
String expression = @"(X + Y) * Z;";

ExpressionCompiler compiler = new ExpressionCompiler();
ExpressionContext context = new ExpressionContext();

context.Bind("X", 1);
context.Bind("Y", 2);
context.Bind("Z", 3);

Int32 result = compiler.Evaluate<Int32>(expression, context);      // result == 9
```
You can also bind parameters through anonymous class.
``` CSharp
String expression = @"(X + Y) * Z;";

ExpressionCompiler compiler = new ExpressionCompiler();
ExpressionContext context = new ExpressionContext(new {
    X = 1,
    Y = 2,
    Z = 3
});

Int32 result = compiler.Evaluate<Int32>(expression, context);      // result == 9
```
