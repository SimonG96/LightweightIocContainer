// Author: Gockner, Simon
// Created: 2019-11-22
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// The <see cref="IDefaultRegistration{TInterface,TImplementation}"/> to register a <see cref="Type"/> for the Interface it implements
    /// </summary>
    /// <typeparam name="TInterface">The <see cref="Type"/> of the interface</typeparam>
    /// <typeparam name="TImplementation">The <see cref="Type"/> of the implementation</typeparam>
    public interface IDefaultRegistration<TInterface, TImplementation> : ITypedRegistrationBase<TInterface, TImplementation> where TImplementation : TInterface
    {
        
    }
}