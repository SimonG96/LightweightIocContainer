// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Interfaces
{
    /// <summary>
    /// The main container that carries all the <see cref="IRegistrationBase"/>s and can resolve all the types you'll ever want
    /// </summary>
    public interface IIocContainer : IDisposable
    {
        /// <summary>
        /// Install the given installers for the current <see cref="IIocContainer"/>
        /// </summary>
        /// <param name="installers">The given <see cref="IIocInstaller"/>s</param>
        /// <returns>An instance of the current <see cref="IIocContainer"/></returns>
        IIocContainer Install(params IIocInstaller[] installers);

        /// <summary>
        /// Add the <see cref="IRegistrationBase"/> to the the <see cref="IIocContainer"/>
        /// </summary>
        /// <param name="registration">The given <see cref="IRegistrationBase"/></param>
        /// <exception cref="MultipleRegistrationException">The Type is already registered in this <see cref="IIocContainer"/></exception>
        void Register(IRegistrationBase registration);

        /// <summary>
        /// Gets an instance of the given type
        /// </summary>
        /// <typeparam name="T">The given type</typeparam>
        /// <returns>An instance of the given type</returns>
        T Resolve<T>();

        /// <summary>
        /// Gets an instance of the given type
        /// </summary>
        /// <typeparam name="T">The given type</typeparam>
        /// <param name="arguments">The constructor arguments</param>
        /// <returns>An instance of the given type</returns>
        T Resolve<T>(params object[] arguments);

        /// <summary>
        /// Gets an instance of the given type
        /// </summary>
        /// <param name="type">The given type</param>
        /// <param name="arguments">The constructor arguments</param>
        /// <returns>An instance of the given type</returns>
        /// <exception cref="InternalResolveException">Could not find function <see cref="IocContainer.ResolveInternal{T}"/></exception>
        object Resolve(Type type, object[] arguments);
    }
}