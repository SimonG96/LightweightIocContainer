// Author: Gockner, Simon
// Created: 2021-12-08
// Copyright(c) 2021 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.ResolvePlaceholders;

/// <summary>
/// An internal placeholder that is used to hold types that need to be resolved during the resolving process
/// </summary>
internal class InternalToBeResolvedPlaceholder(Type resolvedType, IRegistration resolvedRegistration, List<object?>? parameters) : IInternalToBeResolvedPlaceholder
{
    /// <summary>
    /// The <see cref="Type"/> to be resolved
    /// </summary>
    public Type ResolvedType { get; } = resolvedType;

    /// <summary>
    /// The <see cref="IRegistration"/> to be resolved
    /// </summary>
    public IRegistration ResolvedRegistration { get; } = resolvedRegistration;

    /// <summary>
    /// The parameters needed to resolve the <see cref="ResolvedRegistration"/>
    /// </summary>
    public List<object?>? Parameters { get; } = parameters;
}