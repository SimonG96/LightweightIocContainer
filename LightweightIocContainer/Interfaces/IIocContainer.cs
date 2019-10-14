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
        /// Register an Interface with a Type that implements it/>
        /// </summary>
        /// <typeparam name="TInterface">The Interface to register</typeparam>
        /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IDefaultRegistration{TInterface}"/></param>
        void Register<TInterface, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface;

        /// <summary>
        /// Register a <see cref="Type"/> without an interface/>
        /// </summary>
        /// <typeparam name="TImplementation">The <see cref="Type"/> to register</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IDefaultRegistration{TInterface}"/></param>
        void Register<TImplementation>(Lifestyle lifestyle = Lifestyle.Transient);

        /// <summary>
        /// Register an Interface with a Type that implements it as a multiton/>
        /// </summary>
        /// <typeparam name="TInterface">The Interface to register</typeparam>
        /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
        /// <typeparam name="TScope">The Type of the multiton scope</typeparam>
        void Register<TInterface, TImplementation, TScope>() where TImplementation : TInterface;

        /// <summary>
        /// Register an Interface with a Type that implements it/>
        /// </summary>
        /// <param name="tInterface">The Interface to register</param>
        /// <param name="tImplementation">The Type that implements the interface</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IDefaultRegistration{TInterface}"/></param>
        void Register(Type tInterface, Type tImplementation, Lifestyle lifestyle = Lifestyle.Transient);

        /// <summary>
        /// Register a <see cref="Type"/> without an interface/>
        /// </summary>
        /// <param name="tImplementation">The <see cref="Type"/> to register</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IDefaultRegistration{TInterface}"/></param>
        void Register(Type tImplementation, Lifestyle lifestyle = Lifestyle.Transient);

        /// <summary>
        /// Register an Interface with a Type that implements it as a multiton/>
        /// </summary>
        /// <param name="tInterface">The Interface to register</param>
        /// <param name="tImplementation">The Type that implements the interface</param>
        /// <param name="tScope">The Type of the multiton scope</param>
        void Register(Type tInterface, Type tImplementation, Type tScope);

        /// <summary>
        /// Register an Interface as an abstract typed factory/>
        /// </summary>
        /// <typeparam name="TFactory">The abstract typed factory to register</typeparam>
        void RegisterFactory<TFactory>();

        /// <summary>
        /// Register an Interface as an abstract typed factory/>
        /// </summary>
        /// <param name="tFactory">The abstract typed factory to register</param>
        void RegisterFactory(Type tFactory);

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
        /// Gets an instance of the given <see cref="Type"/>
        /// </summary>
        /// <param name="type">The given <see cref="Type"/></param>
        /// <param name="arguments">The constructor arguments</param>
        /// <returns>An instance of the given <see cref="Type"/></returns>
        /// <exception cref="InternalResolveException">Could not find function <see cref="IocContainer.ResolveInternal{T}"/></exception>
        object Resolve(Type type, object[] arguments);

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