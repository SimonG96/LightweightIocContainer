// Author: Gockner, Simon
// Created: 2019-11-22
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// The <see cref="DefaultRegistration{TInterface}"/> to register a <see cref="Type"/> for the Interface it implements
    /// </summary>
    /// <typeparam name="TInterface">The <see cref="Type"/> of the interface</typeparam>
    public class DefaultRegistration<TInterface> : TypedRegistrationBase<TInterface>, IDefaultRegistration<TInterface>
    {
        /// <summary>
        /// The <see cref="DefaultRegistration{TInterface}"/> to register a <see cref="Type"/> for the Interface it implements
        /// </summary>
        /// <param name="interfaceType">The <see cref="Type"/> of the interface</param>
        /// <param name="implementationType">The <see cref="Type"/> of the implementation</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> of the <see cref="RegistrationBase{TInterface}"/></param>
        public DefaultRegistration(Type interfaceType, Type implementationType, Lifestyle lifestyle)
            : base(interfaceType, implementationType, lifestyle)
        {
            Name = $"{InterfaceType.Name}, {ImplementationType.Name}, Lifestyle: {Lifestyle.ToString()}";
        }

        /// <summary>
        /// This <see cref="Action{T}"/> is invoked when an instance of this type is created.
        /// <para>Can be set in the <see cref="IIocInstaller"/> by calling <see cref="OnCreate"/></para>
        /// </summary>
        public Action<TInterface> OnCreateAction { get; private set; }


        /// <summary>
        /// Pass an <see cref="Action{T}"/> that will be invoked when an instance of this <see cref="Type"/> is created
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/></param>
        /// <returns>The current instance of this <see cref="IRegistrationBase{TInterface}"/></returns>
        public IRegistrationBase<TInterface> OnCreate(Action<TInterface> action)
        {
            OnCreateAction = action;
            return this;
        }
    }
}