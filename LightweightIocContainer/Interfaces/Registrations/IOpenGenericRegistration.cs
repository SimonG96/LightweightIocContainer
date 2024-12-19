// Author: Simon Gockner
// Created: 2020-09-18
// Copyright(c) 2020 SimonG. All Rights Reserved.

namespace LightweightIocContainer.Interfaces.Registrations;

/// <summary>
/// <see cref="IRegistration"/> for open generic types
/// </summary>
public interface IOpenGenericRegistration : ITypedRegistration
{
    internal Type CreateGenericImplementationType<T>();
}