// Author: Simon Gockner
// Created: 2019-12-07
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System.Collections.Generic;
using LightweightIocContainer.Interfaces.Registrations.FluentProviders;

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// An <see cref="IRegistrationBase{TInterface}"/> to register multiple interfaces for on implementation type
    /// </summary>
    /// <typeparam name="TInterface1">The first interface</typeparam>
    /// <typeparam name="TInterface2">The second interface</typeparam>
    public interface IMultipleRegistration<TInterface1, TInterface2> : ITypedRegistrationBase<TInterface1>, IOnCreate<TInterface1, TInterface2>
    {
        /// <summary>
        /// A <see cref="List{T}"/> of <see cref="IRegistration"/>s that are registered within this <see cref="IMultipleRegistration{TInterface1,TInterface2}"/>
        /// </summary>
        List<IRegistration> Registrations { get; }
    }
}