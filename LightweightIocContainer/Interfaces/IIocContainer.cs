// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Interfaces
{
    /// <summary>
    /// The main container that carries all the <see cref="IRegistration"/>s and can resolve all the types you'll ever want
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
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistrationBase{TInterface}"/></param>
        /// <returns>The created <see cref="IRegistration"/></returns>
        IDefaultRegistration<TInterface, TImplementation> Register<TInterface, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface;

        /// <summary>
        /// Register multiple interfaces for a <see cref="Type"/> that implements them
        /// </summary>
        /// <typeparam name="TInterface1">The base interface to register</typeparam>
        /// <typeparam name="TInterface2">A second interface to register</typeparam>
        /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistrationBase{TInterface}"/></param>
        /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2}"/></returns>
        IMultipleRegistration<TInterface1, TInterface2, TImplementation> Register<TInterface1, TInterface2, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface2, TInterface1;

        /// <summary>
        /// Register multiple interfaces for a <see cref="Type"/> that implements them
        /// </summary>
        /// <typeparam name="TInterface1">The base interface to register</typeparam>
        /// <typeparam name="TInterface2">A second interface to register</typeparam>
        /// <typeparam name="TInterface3">A third interface to register</typeparam>
        /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistrationBase{TInterface}"/></param>
        /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2,TInterface3}"/></returns>
        IMultipleRegistration<TInterface1, TInterface2, TInterface3, TImplementation> Register<TInterface1, TInterface2, TInterface3, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface3, TInterface2, TInterface1;

        /// <summary>
        /// Register multiple interfaces for a <see cref="Type"/> that implements them
        /// </summary>
        /// <typeparam name="TInterface1">The base interface to register</typeparam>
        /// <typeparam name="TInterface2">A second interface to register</typeparam>
        /// <typeparam name="TInterface3">A third interface to register</typeparam>
        /// <typeparam name="TInterface4">A fourth interface to register</typeparam>
        /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistrationBase{TInterface}"/></param>
        /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2,TInterface3,TInterface4}"/></returns>
        IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation> Register<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface4, TInterface3, TInterface2, TInterface1;

        /// <summary>
        /// Register multiple interfaces for a <see cref="Type"/> that implements them
        /// </summary>
        /// <typeparam name="TInterface1">The base interface to register</typeparam>
        /// <typeparam name="TInterface2">A second interface to register</typeparam>
        /// <typeparam name="TInterface3">A third interface to register</typeparam>
        /// <typeparam name="TInterface4">A fourth interface to register</typeparam>
        /// <typeparam name="TInterface5">A fifth interface to register</typeparam>
        /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistrationBase{TInterface}"/></param>
        /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2,TInterface3,TInterface4,TInterface5}"/></returns>
        IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation> Register<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface5, TInterface4, TInterface3, TInterface2, TInterface1;

        /// <summary>
        /// Register a <see cref="Type"/> without an interface
        /// </summary>
        /// <typeparam name="TImplementation">The <see cref="Type"/> to register</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistrationBase{TInterface}"/></param>
        /// <returns>The created <see cref="IRegistration"/></returns>
        ISingleTypeRegistration<TImplementation> Register<TImplementation>(Lifestyle lifestyle = Lifestyle.Transient);

        /// <summary>
        /// Register an Interface with a Type that implements it as a multiton
        /// </summary>
        /// <typeparam name="TInterface">The Interface to register</typeparam>
        /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
        /// <typeparam name="TScope">The Type of the multiton scope</typeparam>
        /// <returns>The created <see cref="IRegistration"/></returns>
        IMultitonRegistration<TInterface, TImplementation> RegisterMultiton<TInterface, TImplementation, TScope>() where TImplementation : TInterface;

        /// <summary>
        /// Register an Interface as an abstract typed factory
        /// </summary>
        /// <typeparam name="TFactory">The abstract typed factory to register</typeparam>
        /// <returns>The created <see cref="IRegistration"/></returns>
        ITypedFactoryRegistration<TFactory> RegisterFactory<TFactory>();

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