// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// The default registration that is used to register a Type for the Interface it implements
    /// </summary>
    /// <typeparam name="TInterface">The registered Interface</typeparam>
    public class DefaultRegistration<TInterface> : IDefaultRegistration<TInterface>
    {
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
        /// The Type of the Interface that is registered with this <see cref="DefaultRegistration{TInterface}"/>
        /// </summary>
        public Type InterfaceType { get; }

        /// <summary>
        /// The Type that implements the <see cref="InterfaceType"/> that is registered with this <see cref="DefaultRegistration{TInterface}"/>
        /// </summary>
        public Type ImplementationType { get; }

        /// <summary>
        /// The Lifestyle of Instances that are created with this <see cref="DefaultRegistration{TInterface}"/>
        /// </summary>
        public Lifestyle Lifestyle { get; }


        /// <summary>
        /// This action is invoked when an instance of this type is created.
        /// <para>Can be set in the <see cref="IInjectorInstaller"/> by calling <see cref="OnCreate"/></para>
        /// </summary>
        public Action<TInterface> OnCreateAction { get; private set; }


        /// <summary>
        /// Pass an action that will be invoked when an instance of this type is created
        /// </summary>
        /// <param name="action">The action</param>
        /// <returns>The current instance of this <see cref="IDefaultRegistration{TInterface}"/></returns>
        public IDefaultRegistration<TInterface> OnCreate(Action<TInterface> action)
        {
            OnCreateAction = action;
            return this;
        }
    }
}