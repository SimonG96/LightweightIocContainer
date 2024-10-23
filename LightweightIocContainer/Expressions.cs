// Author: Simon.Gockner
// Created: 2024-04-17
// Copyright(c) 2024 SimonG. All Rights Reserved.

using System.Linq.Expressions;
using System.Reflection;
using FastExpressionCompiler;

namespace LightweightIocContainer;

internal class Expressions
{
    public T Create<T>(ConstructorInfo constructor, params object?[]? arguments)
    {
        (Expression? expression, IEnumerable<ParameterExpression> parameters) = CreateConstructorExpression(constructor);
        return ((Func<T>) Expression.Lambda(expression, parameters).CompileFast())(); //TODO: Pass arguments to func? (don't use dynamicInvoke if possible), maybe use factory class?
    }

    private (Expression expression, IEnumerable<ParameterExpression> parameters) CreateConstructorExpression(ConstructorInfo constructor)
    {
        ParameterInfo[] parameters = constructor.GetParameters();
        if (parameters.Length == 0)
            return (Expression.New(constructor), []);

        ParameterExpression[] parameterExpressions = new ParameterExpression[parameters.Length];
        for (int i = 0; i < parameters.Length; i++) 
            parameterExpressions[i] = Expression.Parameter(parameters[i].ParameterType);

        return (Expression.New(constructor, parameterExpressions.Cast<Expression>()), parameterExpressions);
    }
}