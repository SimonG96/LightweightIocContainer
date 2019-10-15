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
    /// A factory to register interfaces and factories in an <see cref="IIocInstaller"/> and create the needed <see cref="IRegistrationBase"/>s
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
        /// Register a <see cref="Type"/> without an interface and create a <see cref="IDefaultRegistration{TInterface}"/>
        /// </summary>
        /// <typeparam name="TImplementation">The <see cref="Type"/> to register</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IDefaultRegistration{TInterface}"/></param>
        /// <returns>A new created <see cref="IDefaultRegistration{TInterface}"/> with the given parameters</returns>
        public IDefaultRegistration<TImplementation> Register<TImplementation>(Lifestyle lifestyle)
        {
            if (typeof(TImplementation).IsInterface)
                throw new InvalidRegistrationException("Can't register an interface without its implementation type.");

            return Register<TImplementation, TImplementation>(lifestyle);
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
        /// Register an Interface with a Type that implements it and create a <see cref="IDefaultRegistration{TInterface}"/>
        /// </summary>
        /// <param name="tInterface">The Interface to register</param>
        /// <param name="tImplementation">The Type that implements the interface</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IDefaultRegistration{TInterface}"/></param>
        /// <returns>A new created <see cref="IDefaultRegistration{TInterface}"/> with the given parameters</returns>
        public IRegistrationBase Register(Type tInterface, Type tImplementation, Lifestyle lifestyle)
        {
            Type defaultRegistrationType = typeof(DefaultRegistration<>).MakeGenericType(tInterface);
            return (IRegistrationBase)Activator.CreateInstance(defaultRegistrationType, tInterface, tImplementation, lifestyle);
        }

        /// <summary>
        /// Register a <see cref="Type"/> without an interface and create a <see cref="IDefaultRegistration{TInterface}"/>
        /// </summary>
        /// <param name="tImplementation">The <see cref="Type"/> to register</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IDefaultRegistration{TInterface}"/></param>
        /// <returns>A new created <see cref="IDefaultRegistration{TInterface}"/> with the given parameters</returns>
        public IRegistrationBase Register(Type tImplementation, Lifestyle lifestyle)
        {
            if (tImplementation.IsInterface)
                throw new InvalidRegistrationException("Can't register an interface without its implementation type.");

            return Register(tImplementation, tImplementation, lifestyle);
        }

        /// <summary>
        /// Register an Interface with a Type that implements it as a multiton and create a <see cref="IMultitonRegistration{TInterface}"/>
        /// </summary>
        /// <param name="tInterface">The Interface to register</param>
        /// <param name="tImplementation">The Type that implements the interface</param>
        /// <param name="tScope">The Type of the multiton scope</param>
        /// <returns>A new created <see cref="IMultitonRegistration{TInterface}"/> with the given parameters</returns>
        public IRegistrationBase Register(Type tInterface, Type tImplementation, Type tScope)
        {
            Type multitonRegistrationType = typeof(MultitonRegistration<>).MakeGenericType(tInterface);
            return (IRegistrationBase)Activator.CreateInstance(multitonRegistrationType, tInterface, tImplementation, tScope);
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

        /// <summary>
        /// Register an Interface as an abstract typed factory and create a <see cref="ITypedFactoryRegistration{TFactory}"/>
        /// </summary>
        /// <param name="tFactory">The abstract typed factory to register</param>
        /// <returns>A new created <see cref="ITypedFactoryRegistration{TFactory}"/> with the given parameters</returns>
        public IRegistrationBase RegisterFactory(Type tFactory)
        {
            Type factoryRegistrationType = typeof(TypedFactoryRegistration<>).MakeGenericType(tFactory);
            return (IRegistrationBase)Activator.CreateInstance(factoryRegistrationType, tFactory, _iocContainer);
        }

        public IUnitTestCallbackRegistration<TInterface> RegisterUnitTestCallback<TInterface>(ResolveCallback<TInterface> unitTestResolveCallback)
        {
            return new UnitTestCallbackRegistration<TInterface>(typeof(TInterface), unitTestResolveCallback);
        }
    }
}