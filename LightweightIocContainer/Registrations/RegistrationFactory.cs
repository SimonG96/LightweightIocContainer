// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// A factory to register interfaces and factories in an <see cref="IIocInstaller"/> and create the needed <see cref="IRegistrationBase"/>s
    /// </summary>
    public static class RegistrationFactory
    {
        /// <summary>
        /// Register an Interface with a Type that implements it and create a <see cref="IDefaultRegistration{TInterface}"/>
        /// </summary>
        /// <typeparam name="TInterface">The Interface to register</typeparam>
        /// <typeparam name="TImplementation">The Type that implements the <see cref="TInterface"/></typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IDefaultRegistration{TInterface}"/></param>
        /// <returns>A new created <see cref="IDefaultRegistration{TInterface}"/> with the given parameters</returns>
        public static IDefaultRegistration<TInterface> Register<TInterface, TImplementation>(Lifestyle lifestyle = Lifestyle.Transient) where TImplementation : TInterface
        {
            return new DefaultRegistration<TInterface>(typeof(TInterface), typeof(TImplementation), lifestyle);
        }

        /// <summary>
        /// Register an Interface as an abstract typed factory and create a <see cref="ITypedFactoryRegistration{TFactory}"/>
        /// </summary>
        /// <typeparam name="TFactory">The abstract typed factory to register</typeparam>
        /// <param name="container">The current <see cref="IIocContainer"/></param>
        /// <returns>A new created <see cref="ITypedFactoryRegistration{TFactory}"/> with the given parameters</returns>
        public static ITypedFactoryRegistration<TFactory> RegisterFactory<TFactory>(IIocContainer container) //TODO: Find a nicer way to inject the container into `TypedFactoryRegistration`
        {
            return new TypedFactoryRegistration<TFactory>(typeof(TFactory), container);
        }
    }
}