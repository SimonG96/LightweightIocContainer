// Author: Gockner, Simon
// Created: 2021-12-14
// Copyright(c) 2021 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.ResolvePlaceholders;

/// <summary>
/// An internal placeholder that is used to hold factory methods for types that need to be resolved during the resolve process
/// </summary>
internal class InternalFactoryMethodPlaceholder<T> : IInternalToBeResolvedPlaceholder
{
    public InternalFactoryMethodPlaceholder(ISingleTypeRegistration<T> singleTypeRegistration)
    {
        ResolvedType = singleTypeRegistration.InterfaceType;
        SingleTypeRegistration = singleTypeRegistration;
    }

    /// <summary>
    /// The <see cref="Type"/> to be resolved
    /// </summary>
    public Type ResolvedType { get; }
    
    /// <summary>
    /// The <see cref="ISingleTypeRegistration{T}"/>
    /// </summary>
    public ISingleTypeRegistration<T> SingleTypeRegistration { get; }
}