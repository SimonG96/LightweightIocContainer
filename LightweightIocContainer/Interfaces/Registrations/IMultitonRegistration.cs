// Author: Gockner, Simon
// Created: 2019-06-07
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// A base <see cref="IMultitonRegistration{TInterface}"/> without implementation
    /// </summary>
    public interface IMultitonRegistration<TInterface> : IRegistrationBase<TInterface>
    {
        /// <summary>
        /// The <see cref="Type"/> of the multiton scope
        /// </summary>
        Type Scope { get; }
    }

    /// <summary>
    /// The registration that is used to register a multiton
    /// </summary>
    /// <typeparam name="TInterface">The registered interface</typeparam>
    /// <typeparam name="TImplementation">The registered implementation</typeparam>
    public interface IMultitonRegistration<TInterface, TImplementation> : IMultitonRegistration<TInterface>, IDefaultRegistration<TInterface, TImplementation> where TImplementation : TInterface
    {
        
    }
}