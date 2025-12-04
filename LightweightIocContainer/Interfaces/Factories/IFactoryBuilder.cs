// Author: Simon.Gockner
// Created: 2025-12-02
// Copyright(c) 2025 SimonG. All Rights Reserved.

namespace LightweightIocContainer.Interfaces.Factories;

/// <summary>
/// Internal class used by the factory source generator to create factory instances
/// </summary>
public interface IFactoryBuilder
{
    /// <summary>
    /// Internal method used by the factory source generator to create factory instances
    /// </summary>
    /// <param name="container">The current instance of the <see cref="IocContainer"/></param>
    /// <typeparam name="TFactory">The type of the factory</typeparam>
    /// <returns>The created factory instance</returns>
    TFactory Create<TFactory>(IocContainer container);
}