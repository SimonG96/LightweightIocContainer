// Author: Simon Gockner
// Created: 2019-12-14
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// A <see cref="IRegistrationBase{TInterface}"/> that implements a <see cref="Type"/>
    /// </summary>
    public abstract class TypedRegistrationBase<TInterface> : RegistrationBase<TInterface>, ITypedRegistrationBase<TInterface>
    {
        /// <summary>
        /// A <see cref="IRegistrationBase{TInterface}"/> that implements a <see cref="Type"/>
        /// </summary>
        /// <param name="interfaceType">The <see cref="Type"/> of the interface</param>
        /// <param name="implementationType">The <see cref="Type"/> of the implementation type</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> of this <see cref="IRegistrationBase{TInterface}"/></param>
        protected TypedRegistrationBase(Type interfaceType, Type implementationType, Lifestyle lifestyle)
            : base(interfaceType, lifestyle)
        {
            ImplementationType = implementationType;
        }

        /// <summary>
        /// The <see cref="Type"/> that implements the <see cref="IRegistration.InterfaceType"/> that is registered with this <see cref="IRegistrationBase{TInterface}"/>
        /// </summary>
        public Type ImplementationType { get; }
    }
}