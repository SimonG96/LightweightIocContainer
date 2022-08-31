// Author: Gockner, Simon
// Created: 2019-11-04
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;

namespace LightweightIocContainer.Exceptions;

/// <summary>
/// No matching constructor was found for the given or resolvable arguments
/// </summary>
internal class NoMatchingConstructorFoundException : IocContainerException
{
    /// <summary>
    /// No matching constructor was found for the given or resolvable arguments
    /// </summary>
    /// <param name="type">The <see cref="Type"/> with no matching constructor</param>
    public NoMatchingConstructorFoundException(Type type)
        : base($"No matching constructor for {type} found.")
    {
        Type = type;
        InnerExceptions = new List<Exception>();
    }


    /// <summary>
    /// The <see cref="Type"/> with no matching constructor
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Add an inner exception to the <see cref="IocContainerException.InnerExceptions"/>
    /// </summary>
    /// <param name="exception">The <see cref="ConstructorNotMatchingException"/></param>
    public void AddInnerException(ConstructorNotMatchingException exception) => InnerExceptions.Add(exception);
}