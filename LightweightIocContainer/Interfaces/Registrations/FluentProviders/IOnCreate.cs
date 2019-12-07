// Author: Simon Gockner
// Created: 2019-12-07
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Installers;

namespace LightweightIocContainer.Interfaces.Registrations.FluentProviders
{
    /// <summary>
    /// Provides an <see cref="OnCreate"/> method to an <see cref="IRegistrationBase{TInterface}"/>
    /// </summary>
    /// <typeparam name="TInterface">The registered interface</typeparam>
    public interface IOnCreate<TInterface>
    {
        /// <summary>
        /// This <see cref="Action"/> is invoked when an instance of this type is created.
        /// <para>Can be set in the <see cref="IIocInstaller"/> by calling <see cref="OnCreate"/></para>
        /// </summary>
        Action<TInterface> OnCreateAction { get; }

        /// <summary>
        /// Pass an <see cref="Action{T}"/> that will be invoked when an instance of this type is created
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/></param>
        /// <returns>The current instance of this <see cref="IRegistrationBase{TInterface}"/></returns>
        IRegistrationBase<TInterface> OnCreate(Action<TInterface> action);
    }
}