// Author: Gockner, Simon
// Created: 2019-11-22
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// The <see cref="IDefaultRegistration{TInterface}"/> to register a <see cref="Type"/> for the Interface it implements
    /// </summary>
    /// <typeparam name="TInterface">The <see cref="Type"/> of the interface</typeparam>
    public interface IDefaultRegistration<TInterface> : IRegistrationBase<TInterface>
    {
        /// <summary>
        /// The <see cref="Type"/> that implements the <see cref="IRegistration.InterfaceType"/> that is registered with this <see cref="IRegistrationBase{TInterface}"/>
        /// </summary>
        Type ImplementationType { get; }
    }
}