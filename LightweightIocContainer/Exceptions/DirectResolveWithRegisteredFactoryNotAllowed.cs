// Author: Gockner, Simon
// Created: 2022-08-31
// Copyright(c) 2022 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Exceptions;

/// <summary>
/// A direct resolve with a registered factory is not allowed
/// </summary>
public class DirectResolveWithRegisteredFactoryNotAllowed : IocContainerException
{
    /// <summary>
    /// A direct resolve with a registered factory is not allowed
    /// </summary>
    /// <param name="type">The type that can't be resolved directly</param>
    public DirectResolveWithRegisteredFactoryNotAllowed(Type type)
        : base($"A direct resolve of type {type} is not allowed! Use the registered factory!") =>
        Type = type;

    /// <summary>
    /// The <see cref="Type"/> that can't be resolved directly
    /// </summary>
    public Type Type { get; }
}