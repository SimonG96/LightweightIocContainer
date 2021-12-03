// Author: Simon Gockner
// Created: 2019-12-08
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Registrations.Fluent;

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// A base <see cref="ITypedRegistration"/> without generic interface and implementation
    /// </summary>
    public interface ITypedRegistration : IRegistrationBase
    {
        /// <summary>
        /// The <see cref="Type"/> that implements the <see cref="IRegistration.InterfaceType"/> that is registered with this <see cref="IRegistration"/>
        /// </summary>
        Type ImplementationType { get; }
    }
    
    /// <summary>
    /// A <see cref="IRegistration"/> that implements a <see cref="Type"/>
    /// </summary>
    public interface ITypedRegistration<TInterface, TImplementation> : ITypedRegistration, IOnCreate<TInterface, TImplementation> where TImplementation : TInterface
    {
        
    }
}