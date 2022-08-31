// Author: Gockner, Simon
// Created: 2019-11-05
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightweightIocContainer.Exceptions;

/// <summary>
/// A circular dependency was detected during <see cref="IocContainer.Resolve{T}()"/>
/// </summary>
internal class CircularDependencyException : IocContainerException
{
    /// <summary>
    /// A circular dependency was detected during <see cref="IocContainer.Resolve{T}()"/>
    /// </summary>
    /// <param name="resolvingType">The currently resolving <see cref="Type"/></param>
    /// <param name="resolveStack">The resolve stack at the time the <see cref="CircularDependencyException"/> was thrown</param>
    public CircularDependencyException(Type resolvingType, List<Type> resolveStack)
    {
        ResolvingType = resolvingType;
        ResolveStack = resolveStack;
    }


    /// <summary>
    /// The currently resolving <see cref="Type"/>
    /// </summary>
    public Type ResolvingType { get; }

    /// <summary>
    /// The resolve stack at the time the <see cref="CircularDependencyException"/> was thrown
    /// </summary>
    public List<Type> ResolveStack { get; }

    /// <summary>
    /// The exception message
    /// </summary>
    public override string Message
    {
        get
        {
            StringBuilder message = new($"Circular dependency has been detected when trying to resolve `{ResolvingType}`.\n");
            if (!ResolveStack.Any())
                return message.ToString();

            message.Append("Resolve stack that resulted in the circular dependency:\n");
            message.Append($"\t`{ResolvingType}` resolved as dependency of\n");

            for (int i = ResolveStack.Count - 1; i >= 1 ; i--)
                message.Append($"\t`{ResolveStack[i]}` resolved as dependency of\n");

            message.Append($"\t`{ResolveStack[0]}` which is the root type being resolved.");
            return message.ToString();
        }
    }
}