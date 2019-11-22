// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// A factory to register interfaces and factories in an <see cref="IIocInstaller"/> and create the needed <see cref="IRegistration"/>s
    /// </summary>
    internal class RegistrationFactory
    {
        private readonly IIocContainer _iocContainer;

        internal RegistrationFactory(IIocContainer container)
        {
            _iocContainer = container;
        }

        /// <summary>
        /// Register an Interface with a Type that implements it and create a <see cref="IDefaultRegistration{TInterface}"/>
        /// </summary>
        /// <typeparam name="TInterface">The Interface to register</typeparam>
        /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IDefaultRegistration{TInterface}"/></param>
        /// <returns>A new created <see cref="IDefaultRegistration{TInterface}"/> with the given parameters</returns>
        public IDefaultRegistration<TInterface> Register<TInterface, TImplementation>(Lifestyle lifestyle) where TImplementation : TInterface
        {
            return new DefaultRegistration<TInterface>(typeof(TInterface), typeof(TImplementation), lifestyle);
        }

        /// <summary>
        /// Register a <see cref="Type"/> without an interface and create a <see cref="ISingleTypeRegistration{TInterface}"/>
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to register</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="ISingleTypeRegistration{TInterface}"/></param>
        /// <returns>A new created <see cref="ISingleTypeRegistration{TInterface}"/> with the given parameters</returns>
        public ISingleTypeRegistration<T> Register<T>(Lifestyle lifestyle)
        {
            return new SingleTypeRegistration<T>(typeof(T), lifestyle);
        }

        /// <summary>
        /// Register an Interface with a Type that implements it as a multiton and create a <see cref="IMultitonRegistration{TInterface}"/>
        /// </summary>
        /// <typeparam name="TInterface">The Interface to register</typeparam>
        /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
        /// <typeparam name="TScope">The Type of the multiton scope</typeparam>
        /// <returns>A new created <see cref="IMultitonRegistration{TInterface}"/> with the given parameters</returns>
        public IMultitonRegistration<TInterface> Register<TInterface, TImplementation, TScope>() where TImplementation : TInterface
        {
            return new MultitonRegistration<TInterface>(typeof(TInterface), typeof(TImplementation), typeof(TScope));
        }

        /// <summary>
        /// Register an Interface as an abstract typed factory and create a <see cref="ITypedFactoryRegistration{TFactory}"/>
        /// </summary>
        /// <typeparam name="TFactory">The abstract typed factory to register</typeparam>
        /// <returns>A new created <see cref="ITypedFactoryRegistration{TFactory}"/> with the given parameters</returns>
        public ITypedFactoryRegistration<TFactory> RegisterFactory<TFactory>()
        {
            return new TypedFactoryRegistration<TFactory>(typeof(TFactory), _iocContainer);
        }

        public IUnitTestCallbackRegistration<TInterface> RegisterUnitTestCallback<TInterface>(ResolveCallback<TInterface> unitTestResolveCallback)
        {
            return new UnitTestCallbackRegistration<TInterface>(typeof(TInterface), unitTestResolveCallback);
        }
    }
}