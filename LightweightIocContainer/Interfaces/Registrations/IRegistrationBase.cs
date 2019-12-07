// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces.Registrations.FluentProviders;

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// The <see cref="IRegistrationBase{TInterface}"/> that is used to register an Interface
    /// </summary>
    /// <typeparam name="TInterface">The registered Interface</typeparam>
    public interface IRegistrationBase<TInterface> : IRegistration, IOnCreate<TInterface>, IWithParameters<TInterface>
    {
        /// <summary>
        /// The Lifestyle of Instances that are created with this <see cref="IRegistrationBase{TInterface}"/>
        /// </summary>
        Lifestyle Lifestyle { get; }
    }
}