// Author: simon.gockner
// Created: 2019-05-21
// Copyright(c) 2019 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces;

namespace LightweightIocContainer.Exceptions;

/// <summary>
/// The <see cref="System.Type"/> is not registered in this <see cref="IIocContainer"/>
/// </summary>
internal class TypeNotRegisteredException : IocContainerException
{
    /// <summary>
    /// The <see cref="System.Type"/> is not registered in this <see cref="IIocContainer"/>
    /// </summary>
    /// <param name="type">The unregistered <see cref="System.Type"/></param>
    public TypeNotRegisteredException(Type type)
        : base($"Type {type.Name} is not registered in this IocContainer.") =>
        Type = type;

    /// <summary>
    /// The unregistered <see cref="System.Type"/>
    /// </summary>
    public Type Type { get; }
}