// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Installers;

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// The default registration that is used to register a Type for the Interface it implements
    /// </summary>
    /// <typeparam name="TInterface">The registered Interface</typeparam>
    public interface IDefaultRegistration<TInterface> : IRegistrationBase
    {
        /// <summary>
        /// The Type that implements the <see cref="IRegistrationBase.InterfaceType"/> that is registered with this <see cref="IDefaultRegistration{TInterface}"/>
        /// </summary>
        Type ImplementationType { get; }

        /// <summary>
        /// The Lifestyle of Instances that are created with this <see cref="IDefaultRegistration{TInterface}"/>
        /// </summary>
        Lifestyle Lifestyle { get; }

        /// <summary>
        /// This action is invoked when an instance of this type is created.
        /// <para>Can be set in the <see cref="IIocInstaller"/> by calling <see cref="OnCreate"/></para>
        /// </summary>
        Action<TInterface> OnCreateAction { get; }

        /// <summary>
        /// Pass an action that will be invoked when an instance of this type is created
        /// </summary>
        /// <param name="action">The action</param>
        /// <returns>The current instance of this <see cref="IDefaultRegistration{TInterface}"/></returns>
        IDefaultRegistration<TInterface> OnCreate(Action<TInterface> action);
    }
}