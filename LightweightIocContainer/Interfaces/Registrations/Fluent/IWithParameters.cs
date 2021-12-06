// Author: Simon Gockner
// Created: 2019-12-07
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces.Installers;

namespace LightweightIocContainer.Interfaces.Registrations.Fluent
{
    /// <summary>
    /// Provides a <see cref="WithParameters(object[])"/> method to an <see cref="IRegistration"/>
    /// </summary>
    public interface IWithParameters
    {
        /// <summary>
        /// Pass parameters that will be used to<see cref="IIocContainer.Resolve{T}()"/> an instance of this <see cref="IRegistration.InterfaceType"/>
        /// <para>Parameters set with this method are always inserted at the beginning of the argument list if more parameters are given when resolving</para>
        /// </summary>
        /// <param name="parameters">The parameters</param>
        /// <returns>The current instance of this <see cref="IRegistrationBase"/></returns>
        /// <exception cref="InvalidRegistrationException"><see cref="IWithParametersInternal.Parameters"/> are already set or no parameters given</exception>
        IRegistrationBase WithParameters(params object[] parameters);

        /// <summary>
        /// Pass parameters that will be used to<see cref="IIocContainer.Resolve{T}()"/> an instance of this <see cref="IRegistration.InterfaceType"/>
        /// <para>Parameters set with this method are inserted at the position in the argument list that is passed with the parameter if more parameters are given when resolving</para>
        /// </summary>
        /// <param name="parameters">The parameters with their position</param>
        /// <returns>The current instance of this <see cref="IRegistrationBase"/></returns>
        /// <exception cref="InvalidRegistrationException"><see cref="IWithParametersInternal.Parameters"/> are already set or no parameters given</exception>
        IRegistrationBase WithParameters(params (int index, object parameter)[] parameters);
    }

    internal interface IWithParametersInternal : IWithParameters
    {
        /// <summary>
        /// An <see cref="Array"/> of parameters that are used to <see cref="IIocContainer.Resolve{T}()"/> an instance of this <see cref="IRegistration.InterfaceType"/>
        /// <para>Can be set in the <see cref="IIocInstaller"/> by calling <see cref="IWithParameters.WithParameters(object[])"/></para>
        /// </summary>
        object[] Parameters { get; }
    }
}