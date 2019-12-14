// Author: Simon Gockner
// Created: 2019-12-08
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// A <see cref="IRegistrationBase{TInterface}"/> that implements a <see cref="Type"/>
    /// </summary>
    public interface ITypedRegistrationBase<TInterface> : IRegistrationBase<TInterface>
    {
        /// <summary>
        /// The <see cref="Type"/> that implements the <see cref="IRegistration.InterfaceType"/> that is registered with this <see cref="IRegistrationBase{TInterface}"/>
        /// </summary>
        Type ImplementationType { get; }
    }
}