// Author: Gockner, Simon
// Created: 2019-06-07
// Copyright(c) 2019 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces;

namespace LightweightIocContainer.Exceptions;

/// <summary>
/// The <see cref="System.Type"/> is already registered differently in this <see cref="IIocContainer"/>
/// </summary>
internal class MultipleRegistrationException : IocContainerException
{
    /// <summary>
    /// The <see cref="System.Type"/> is already registered differently in this <see cref="IIocContainer"/>
    /// </summary>
    /// <param name="type">The <see cref="System.Type"/> that is already registered in this <see cref="IIocContainer"/></param>
    public MultipleRegistrationException(Type type)
        : base($"Type {type.Name} is already registered differently in this IocContainer.") =>
        Type = type;

    /// <summary>
    /// The registered <see cref="System.Type"/>
    /// </summary>
    public Type Type { get; }
}