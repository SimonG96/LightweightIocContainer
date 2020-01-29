// Author: Simon Gockner
// Created: 2019-12-14
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Interfaces.Registrations.FluentProviders;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// A <see cref="IRegistrationBase{TInterface}"/> that implements a <see cref="Type"/>
    /// </summary>
    public abstract class TypedRegistrationBase<TInterface, TImplementation> : RegistrationBase<TInterface>, ITypedRegistrationBase<TInterface, TImplementation> where TImplementation : TInterface
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

        /// <summary>
        /// This <see cref="Action"/> is invoked when an instance of this type is created.
        /// <para>Can be set in the <see cref="IIocInstaller"/> by calling <see cref="IOnCreate{TInterface,TImplementation}.OnCreate"/></para>
        /// </summary>
        public Action<object> OnCreateAction { get; private set; }

        /// <summary>
        /// Pass an <see cref="Action{T}"/> that will be invoked when an instance of this type is created
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/></param>
        /// <returns>The current instance of this <see cref="ITypedRegistrationBase{TInterface,TImplementation}"/></returns>
        public virtual ITypedRegistrationBase<TInterface, TImplementation> OnCreate(Action<TImplementation> action)
        {
            OnCreateAction = a => action((TImplementation) a);
            return this;
        }
    }
}