// Author: Gockner, Simon
// Created: 2019-11-22
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Registrations.FluentProviders;

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// The <see cref="IDefaultRegistration{TInterface}"/> to register a <see cref="Type"/> for the Interface it implements
    /// </summary>
    /// <typeparam name="TInterface">The <see cref="Type"/> of the interface</typeparam>
    public interface IDefaultRegistration<TInterface> : ITypedRegistrationBase<TInterface>, IOnCreate<TInterface>
    {
        
    }
}