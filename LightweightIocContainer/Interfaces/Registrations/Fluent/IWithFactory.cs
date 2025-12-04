// Author: Gockner, Simon
// Created: 2021-11-30
// Copyright(c) 2021 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces.Factories;

namespace LightweightIocContainer.Interfaces.Registrations.Fluent;

/// <summary>
/// Provides a <see cref="WithFactory{TFactory}"/> method to an <see cref="IRegistrationBase"/>
/// </summary>
public interface IWithFactory
{
    /// <summary>
    /// Register an abstract typed factory for the <see cref="IRegistrationBase"/> 
    /// </summary>
    /// <typeparam name="TFactory">The type of the abstract typed factory</typeparam>
    /// <returns>The current instance of this <see cref="IRegistrationBase"/></returns>
    IRegistrationBase WithFactory<TFactory>();
        
    /// <summary>
    /// Register a custom implemented factory for the <see cref="IRegistrationBase"/>
    /// </summary>
    /// <typeparam name="TFactoryInterface">The type of the interface for the custom factory</typeparam>
    /// <typeparam name="TFactoryImplementation">The type of the implementation for the custom factory</typeparam>
    /// <returns>The current instance of this <see cref="IRegistrationBase"/></returns>
    IRegistrationBase WithFactory<TFactoryInterface, TFactoryImplementation>() where TFactoryImplementation : TFactoryInterface;

    /// <summary>
    /// Internal method used by the factory source generator to add the generated factory to the registration
    /// </summary>
    /// <param name="factoryBuilder">The factory creator</param>
    /// <typeparam name="TFactory">The type of the generated factory</typeparam>
    void AddGeneratedFactory<TFactory>(IFactoryBuilder factoryBuilder);
}

internal interface IWithFactoryInternal : IWithFactory
{
    /// <summary>
    /// The Factory added with the <see cref="IWithFactory.WithFactory{TFactory}"/> method
    /// </summary>
    ITypedFactory? Factory { get; }
}