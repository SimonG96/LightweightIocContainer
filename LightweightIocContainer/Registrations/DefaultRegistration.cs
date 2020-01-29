// Author: Gockner, Simon
// Created: 2019-11-22
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// The <see cref="DefaultRegistration{TInterface,TImplementation}"/> to register a <see cref="Type"/> for the Interface it implements
    /// </summary>
    /// <typeparam name="TInterface">The <see cref="Type"/> of the interface</typeparam>
    /// <typeparam name="TImplementation">The <see cref="Type"/>of the implementation</typeparam>
    public class DefaultRegistration<TInterface, TImplementation> : TypedRegistrationBase<TInterface, TImplementation>, IDefaultRegistration<TInterface, TImplementation> where TImplementation : TInterface
    {
        /// <summary>
        /// The <see cref="DefaultRegistration{TInterface,TImplementation}"/> to register a <see cref="Type"/> for the Interface it implements
        /// </summary>
        /// <param name="interfaceType">The <see cref="Type"/> of the interface</param>
        /// <param name="implementationType">The <see cref="Type"/> of the implementation</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> of the <see cref="RegistrationBase{TInterface}"/></param>
        public DefaultRegistration(Type interfaceType, Type implementationType, Lifestyle lifestyle)
            : base(interfaceType, implementationType, lifestyle)
        {
            Name = $"{InterfaceType.Name}, {ImplementationType.Name}, Lifestyle: {Lifestyle.ToString()}";
        }
    }
}