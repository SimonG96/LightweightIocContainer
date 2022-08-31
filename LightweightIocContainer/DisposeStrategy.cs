// Author: Gockner, Simon
// Created: 2021-12-15
// Copyright(c) 2021 SimonG. All Rights Reserved.

namespace LightweightIocContainer;

/// <summary>
/// The dispose strategy that is used for a singleton or multiton that implements <see cref="IDisposable"/> and is created by the <see cref="IocContainer"/>
/// </summary>
public enum DisposeStrategy
{
    /// <summary>
    /// No dispose strategy
    /// <remarks>Invalid for singletons or multitons that implement <see cref="IDisposable"/></remarks>
    /// </summary>
    None,
    
    /// <summary>
    /// The application is responsible for correctly disposing the instance. Nothing is done by the <see cref="IocContainer"/>
    /// </summary>
    Application,
    
    /// <summary>
    /// The <see cref="IocContainer"/> is responsible for disposing the instance when itself is disposed
    /// </summary>
    Container
}