// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces.Installers;
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
        /// Register an Interface with a Type that implements it
        /// </summary>
        /// <typeparam name="TInterface">The Interface to register</typeparam>
        /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IDefaultRegistration{TInterface}"/></param>
        /// <returns>The created <see cref="IRegistrationBase"/></returns>
        IDefaultRegistration<TInterface> Register<TInterface, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface;

        /// <summary>
        /// Register a <see cref="Type"/> without an interface
        /// </summary>
        /// <typeparam name="TImplementation">The <see cref="Type"/> to register</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IDefaultRegistration{TInterface}"/></param>
        /// <returns>The created <see cref="IRegistrationBase"/></returns>
        IDefaultRegistration<TImplementation> Register<TImplementation>(Lifestyle lifestyle = Lifestyle.Transient);

        /// <summary>
        /// Register an Interface with a Type that implements it as a multiton
        /// </summary>
        /// <typeparam name="TInterface">The Interface to register</typeparam>
        /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
        /// <typeparam name="TScope">The Type of the multiton scope</typeparam>
        /// <returns>The created <see cref="IRegistrationBase"/></returns>
        IMultitonRegistration<TInterface> Register<TInterface, TImplementation, TScope>() where TImplementation : TInterface;

        /// <summary>
        /// Register an Interface as an abstract typed factory
        /// </summary>
        /// <typeparam name="TFactory">The abstract typed factory to register</typeparam>
        /// <returns>The created <see cref="IRegistrationBase"/></returns>
        ITypedFactoryRegistration<TFactory> RegisterFactory<TFactory>();

        /// <summary>
        /// Register an Interface with an <see cref="ResolveCallback{T}"/> as a callback that is called when <see cref="Resolve{T}()"/> is called
        /// </summary>
        /// <typeparam name="TInterface">The Interface to register</typeparam>
        /// <param name="unitTestCallback">The <see cref="ResolveCallback{T}"/> for the callback</param>
        /// <returns>The created <see cref="IRegistrationBase"/></returns>
        IUnitTestCallbackRegistration<TInterface> RegisterUnitTestCallback<TInterface>(ResolveCallback<TInterface> unitTestCallback);

        /// <summary>
        /// Gets an instance of the given <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <returns>An instance of the given <see cref="Type"/></returns>
        T Resolve<T>();

        /// <summary>
        /// Gets an instance of the given <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <param name="arguments">The constructor arguments</param>
        /// <returns>An instance of the given <see cref="Type"/></returns>
        T Resolve<T>(params object[] arguments);

        /// <summary>
        /// Clear the multiton instances of the given <see cref="Type"/> from the registered multitons list
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to clear the multiton instances</typeparam>
        void ClearMultitonInstances<T>();

        /// <summary>
        /// Is the given <see cref="Type"/> registered with this <see cref="IIocContainer"/>
        /// </summary>
        /// <typeparam name="T">The given <see cref="Type"/></typeparam>
        /// <returns>True if the given <see cref="Type"/> is registered with this <see cref="IIocContainer"/>, false if not</returns>
        bool IsTypeRegistered<T>();
    }
}