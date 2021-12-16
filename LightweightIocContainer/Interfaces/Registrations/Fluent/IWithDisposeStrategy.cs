// Author: Gockner, Simon
// Created: 2021-12-15
// Copyright(c) 2021 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Interfaces.Registrations.Fluent;

/// <summary>
/// Provides a <see cref="LightweightIocContainer.DisposeStrategy"/> to an <see cref="IRegistration"/>
/// </summary>
public interface IWithDisposeStrategy
{
    /// <summary>
    /// Add a <see cref="DisposeStrategy"/> for the <see cref="IRegistrationBase"/>
    /// </summary>
    /// <param name="disposeStrategy">The <see cref="DisposeStrategy"/></param>
    /// <returns>The current instance of this <see cref="IRegistrationBase"/></returns>
    IRegistrationBase WithDisposeStrategy(DisposeStrategy disposeStrategy);
}

internal interface IWithDisposeStrategyInternal : IWithDisposeStrategy
{
    /// <summary>
    /// The <see cref="LightweightIocContainer.DisposeStrategy"/> of singletons/multitons that implement <see cref="IDisposable"/> and are created with this <see cref="IRegistration"/>
    /// </summary>
    DisposeStrategy DisposeStrategy { get; }
}