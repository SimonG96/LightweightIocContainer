// Author: Simon Gockner
// Created: 2020-09-18
// Copyright(c) 2020 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// <see cref="IRegistration"/> for open generic types
    /// </summary>
    public class OpenGenericRegistration : RegistrationBase, IOpenGenericRegistration
    {
        /// <summary>
        /// <see cref="IRegistration"/> for open generic types
        /// </summary>
        /// <param name="interfaceType">The <see cref="Type"/> of the interface</param>
        /// <param name="implementationType">The <see cref="Type"/> of the implementation type</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> of this <see cref="IOpenGenericRegistration"/></param>
        /// <param name="iocContainer">The current instance of the <see cref="IIocContainer"/></param>
        public OpenGenericRegistration(Type interfaceType, Type implementationType, Lifestyle lifestyle, IocContainer iocContainer)
            : base(interfaceType, lifestyle, iocContainer) =>
            ImplementationType = implementationType;

        /// <summary>
        /// The <see cref="Type"/> that implements the <see cref="IRegistration.InterfaceType"/> that is registered with this <see cref="IOpenGenericRegistration"/>
        /// </summary>
        public Type ImplementationType { get; }
    }
}