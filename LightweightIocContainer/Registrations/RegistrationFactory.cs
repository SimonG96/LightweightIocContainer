// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
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
        /// Register an Interface with a Type that implements it and create a <see cref="IDefaultRegistration{TInterface,TImplementation}"/>
        /// </summary>
        /// <typeparam name="TInterface">The Interface to register</typeparam>
        /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IDefaultRegistration{TInterface,TImplementation}"/></param>
        /// <returns>A new created <see cref="IDefaultRegistration{TInterface,TImplementation}"/> with the given parameters</returns>
        public IDefaultRegistration<TInterface, TImplementation> Register<TInterface, TImplementation>(Lifestyle lifestyle) where TImplementation : TInterface
        {
            return new DefaultRegistration<TInterface, TImplementation>(typeof(TInterface), typeof(TImplementation), lifestyle);
        }

        public IOpenGenericRegistration Register(Type tInterface, Type tImplementation, Lifestyle lifestyle)	
        {	
            return new OpenGenericRegistration(tInterface, tImplementation, lifestyle);
        }
        
        /// <summary>
        /// Register multiple interfaces for a <see cref="Type"/> that implements them and create a <see cref="IMultipleRegistration{TInterface1,TInterface2}"/>
        /// </summary>
        /// <typeparam name="TInterface1">The base interface to register</typeparam>
        /// <typeparam name="TInterface2">A second interface to register</typeparam>
        /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistrationBase{TInterface}"/></param>
        /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2}"/></returns>
        public IMultipleRegistration<TInterface1, TInterface2, TImplementation> Register<TInterface1, TInterface2, TImplementation>(Lifestyle lifestyle) where TImplementation : TInterface1, TInterface2
        {
            return new MultipleRegistration<TInterface1, TInterface2, TImplementation>(typeof(TInterface1), typeof(TInterface2), typeof(TImplementation), lifestyle);
        }

        /// <summary>
        /// Register multiple interfaces for a <see cref="Type"/> that implements them and create a <see cref="IMultipleRegistration{TInterface1,TInterface2,TInterface3}"/>
        /// </summary>
        /// <typeparam name="TInterface1">The base interface to register</typeparam>
        /// <typeparam name="TInterface2">A second interface to register</typeparam>
        /// <typeparam name="TInterface3">A third interface to register</typeparam>
        /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistrationBase{TInterface}"/></param>
        /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2,TInterface3}"/></returns>
        public IMultipleRegistration<TInterface1, TInterface2, TInterface3, TImplementation> Register<TInterface1, TInterface2, TInterface3, TImplementation>(Lifestyle lifestyle) where TImplementation : TInterface1, TInterface2, TInterface3
        {
            return new MultipleRegistration<TInterface1, TInterface2, TInterface3, TImplementation>(typeof(TInterface1), typeof(TInterface2), typeof(TInterface3), typeof(TImplementation), lifestyle);
        }

        /// <summary>
        /// Register multiple interfaces for a <see cref="Type"/> that implements them and create a <see cref="IMultipleRegistration{TInterface1,TInterface2,TInterface3,TInterface4}"/>
        /// </summary>
        /// <typeparam name="TInterface1">The base interface to register</typeparam>
        /// <typeparam name="TInterface2">A second interface to register</typeparam>
        /// <typeparam name="TInterface3">A third interface to register</typeparam>
        /// <typeparam name="TInterface4">A fourth interface to register</typeparam>
        /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistrationBase{TInterface}"/></param>
        /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2,TInterface3,TInterface4}"/></returns>
        public IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation> Register<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation>(Lifestyle lifestyle) where TImplementation : TInterface1, TInterface2, TInterface3, TInterface4
        {
            return new MultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation>(typeof(TInterface1), typeof(TInterface2), typeof(TInterface3), typeof(TInterface4), typeof(TImplementation), lifestyle);
        }

        /// <summary>
        /// Register multiple interfaces for a <see cref="Type"/> that implements them and create a <see cref="IMultipleRegistration{TInterface1,TInterface2,TInterface3,TInterface4,TInterface5}"/>
        /// </summary>
        /// <typeparam name="TInterface1">The base interface to register</typeparam>
        /// <typeparam name="TInterface2">A second interface to register</typeparam>
        /// <typeparam name="TInterface3">A third interface to register</typeparam>
        /// <typeparam name="TInterface4">A fourth interface to register</typeparam>
        /// <typeparam name="TInterface5">A fifth interface to register</typeparam>
        /// <typeparam name="TImplementation">The <see cref="Type"/> that implements both interfaces</typeparam>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> for this <see cref="IRegistrationBase{TInterface}"/></param>
        /// <returns>The created <see cref="IMultipleRegistration{TInterface1,TInterface2,TInterface3,TInterface4,TInterface5}"/></returns>
        public IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation> Register<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation>(Lifestyle lifestyle) where TImplementation : TInterface1, TInterface2, TInterface3, TInterface4, TInterface5
        {
            return new MultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation>(typeof(TInterface1), typeof(TInterface2), typeof(TInterface3), typeof(TInterface4), typeof(TInterface5), typeof(TImplementation), lifestyle);
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
        /// Register an Interface with a Type that implements it as a multiton and create a <see cref="IMultitonRegistration{TInterface,TImplementation}"/>
        /// </summary>
        /// <typeparam name="TInterface">The Interface to register</typeparam>
        /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
        /// <typeparam name="TScope">The Type of the multiton scope</typeparam>
        /// <returns>A new created <see cref="IMultitonRegistration{TInterface,TImplementation}"/> with the given parameters</returns>
        public IMultitonRegistration<TInterface, TImplementation> RegisterMultiton<TInterface, TImplementation, TScope>() where TImplementation : TInterface
        {
            return new MultitonRegistration<TInterface, TImplementation>(typeof(TInterface), typeof(TImplementation), typeof(TScope));
        }

        /// <summary>
        /// Register multiple interfaces for a <see cref="Type"/> that implements them as a multiton
        /// </summary>
        /// <typeparam name="TInterface1">The base interface to register</typeparam>
        /// <typeparam name="TInterface2">A second interface to register</typeparam>
        /// <typeparam name="TImplementation">The Type that implements the interface</typeparam>
        /// <typeparam name="TScope">The Type of the multiton scope</typeparam>
        /// <returns>A new created <see cref="IMultipleMultitonRegistration{TInterface1,TInterface2,TImplementation}"/> with the given parameters</returns>
        public IMultipleMultitonRegistration<TInterface1, TInterface2, TImplementation> RegisterMultiton<TInterface1, TInterface2, TImplementation, TScope>() where TImplementation : TInterface1, TInterface2
        {
            return new MultipleMultitonRegistration<TInterface1, TInterface2, TImplementation>(typeof(TInterface1), typeof(TInterface2), typeof(TImplementation), typeof(TScope));
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
    }
}