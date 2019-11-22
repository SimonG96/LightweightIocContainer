// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// The <see cref="RegistrationBase{TInterface}"/> that is used to register an Interface
    /// </summary>
    /// <typeparam name="TInterface">The registered Interface</typeparam>
    public abstract class RegistrationBase<TInterface> : IRegistrationBase<TInterface>
    {
        /// <summary>
        /// The <see cref="RegistrationBase{TInterface}"/> that is used to register an Interface
        /// </summary>
        /// <param name="interfaceType">The <see cref="Type"/> of the Interface</param>
        /// <param name="lifestyle">The <see cref="LightweightIocContainer.Lifestyle"/> of the registration</param>
        protected RegistrationBase(Type interfaceType, Lifestyle lifestyle)
        {
            InterfaceType = interfaceType;
            Lifestyle = lifestyle;
        }

        /// <summary>
        /// The name of the <see cref="RegistrationBase{TInterface}"/>
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The <see cref="Type"/> of the Interface that is registered with this <see cref="RegistrationBase{TInterface}"/>
        /// </summary>
        public Type InterfaceType { get; }

        /// <summary>
        /// The <see cref="LightweightIocContainer.Lifestyle"/> of Instances that are created with this <see cref="RegistrationBase{TInterface}"/>
        /// </summary>
        public Lifestyle Lifestyle { get; }


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