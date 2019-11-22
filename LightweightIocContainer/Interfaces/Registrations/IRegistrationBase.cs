// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces.Installers;

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// The <see cref="IRegistrationBase{TInterface}"/> that is used to register an Interface
    /// </summary>
    /// <typeparam name="TInterface">The registered Interface</typeparam>
    public interface IRegistrationBase<TInterface> : IRegistration
    {
        /// <summary>
        /// The Lifestyle of Instances that are created with this <see cref="IRegistrationBase{TInterface}"/>
        /// </summary>
        Lifestyle Lifestyle { get; }

        /// <summary>
        /// This <see cref="Action{T}"/> is invoked when an instance of this type is created.
        /// <para>Can be set in the <see cref="IIocInstaller"/> by calling <see cref="OnCreate"/></para>
        /// </summary>
        Action<TInterface> OnCreateAction { get; }

        /// <summary>
        /// Pass an <see cref="Action{T}"/> that will be invoked when an instance of this type is created
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/></param>
        /// <returns>The current instance of this <see cref="IRegistrationBase{TInterface}"/></returns>
        IRegistrationBase<TInterface> OnCreate(Action<TInterface> action);

        /// <summary>
        /// An <see cref="Array"/> of parameters that are used to <see cref="IIocContainer.Resolve{T}()"/> an instance of this <see cref="IRegistration.InterfaceType"/>
        /// <para>Can be set in the <see cref="IIocInstaller"/> by calling <see cref="WithParameters(object[])"/></para>
        /// </summary>
        object[] Parameters { get; }

        /// <summary>
        /// Pass parameters that will be used to<see cref="IIocContainer.Resolve{T}()"/> an instance of this <see cref="IRegistration.InterfaceType"/>
        /// <para>Parameters set with this method are always inserted at the beginning of the argument list if more parameters are given when resolving</para>
        /// </summary>
        /// <param name="parameters">The parameters</param>
        /// <returns>The current instance of this <see cref="IRegistrationBase{TInterface}"/></returns>
        /// <exception cref="InvalidRegistrationException"><see cref="Parameters"/> are already set or no parameters given</exception>
        IRegistrationBase<TInterface> WithParameters(params object[] parameters);

        /// <summary>
        /// Pass parameters that will be used to<see cref="IIocContainer.Resolve{T}()"/> an instance of this <see cref="IRegistration.InterfaceType"/>
        /// <para>Parameters set with this method are inserted at the position in the argument list that is passed with the parameter if more parameters are given when resolving</para>
        /// </summary>
        /// <param name="parameters">The parameters with their position</param>
        /// <returns>The current instance of this <see cref="IRegistrationBase{TInterface}"/></returns>
        /// <exception cref="InvalidRegistrationException"><see cref="Parameters"/> are already set or no parameters given</exception>
        IRegistrationBase<TInterface> WithParameters(params (int index, object parameter)[] parameters);

    }
}