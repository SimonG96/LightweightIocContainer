// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// The default registration that is used to register a <see cref="Type"/> for the Interface it implements
    /// </summary>
    /// <typeparam name="TInterface">The registered Interface</typeparam>
    public class DefaultRegistration<TInterface> : IDefaultRegistration<TInterface>
    {
        /// <summary>
        /// The default registration that is used to register a <see cref="Type"/> for the Interface it implements
        /// </summary>
        /// <param name="interfaceType">The <see cref="Type"/> of the Interface</param>
        /// <param name="implementationType">The <see cref="Type"/> of the Implementation</param>
        /// <param name="lifestyle">The <see cref="LightweightIocContainer.Lifestyle"/> of the registration</param>
        public DefaultRegistration(Type interfaceType, Type implementationType, Lifestyle lifestyle)
        {
            InterfaceType = interfaceType;
            ImplementationType = implementationType;
            Lifestyle = lifestyle;

            Name = $"{InterfaceType.Name}, {ImplementationType.Name}, Lifestyle: {Lifestyle.ToString()}";
        }

        /// <summary>
        /// The name of the <see cref="DefaultRegistration{TInterface}"/>
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The <see cref="Type"/> of the Interface that is registered with this <see cref="DefaultRegistration{TInterface}"/>
        /// </summary>
        public Type InterfaceType { get; }

        /// <summary>
        /// The <see cref="Type"/> that implements the <see cref="InterfaceType"/> that is registered with this <see cref="DefaultRegistration{TInterface}"/>
        /// </summary>
        public Type ImplementationType { get; }

        /// <summary>
        /// The <see cref="LightweightIocContainer.Lifestyle"/> of Instances that are created with this <see cref="DefaultRegistration{TInterface}"/>
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
        /// <returns>The current instance of this <see cref="IDefaultRegistration{TInterface}"/></returns>
        public IDefaultRegistration<TInterface> OnCreate(Action<TInterface> action)
        {
            OnCreateAction = action;
            return this;
        }
    }
}