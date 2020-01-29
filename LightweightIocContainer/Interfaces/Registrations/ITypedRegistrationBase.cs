// Author: Simon Gockner
// Created: 2019-12-08
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Registrations.FluentProviders;

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// A base <see cref="ITypedRegistrationBase{TInterface}"/> without implementation
    /// </summary>
    public interface ITypedRegistrationBase<TInterface> : IRegistrationBase<TInterface>
    {
        /// <summary>
        /// The <see cref="Type"/> that implements the <see cref="IRegistration.InterfaceType"/> that is registered with this <see cref="IRegistrationBase{TInterface}"/>
        /// </summary>
        Type ImplementationType { get; }
    }

    /// <summary>
    /// A <see cref="IRegistrationBase{TInterface}"/> that implements a <see cref="Type"/>
    /// </summary>
    public interface ITypedRegistrationBase<TInterface, TImplementation> : ITypedRegistrationBase<TInterface>, IOnCreate<TInterface, TImplementation> where TImplementation : TInterface
    {
        
    }
}