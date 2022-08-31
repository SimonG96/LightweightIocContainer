// Author: Gockner, Simon
// Created: 2021-12-14
// Copyright(c) 2021 SimonG. All Rights Reserved.

namespace LightweightIocContainer.ResolvePlaceholders;

/// <summary>
/// An internal placeholder that is used to hold types that need to be resolved during the resolving process
/// </summary>
internal interface IInternalToBeResolvedPlaceholder
{
    /// <summary>
    /// The <see cref="Type"/> to be resolved
    /// </summary>
    Type ResolvedType { get; }
}