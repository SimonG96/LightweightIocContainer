// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Linq;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Factories;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Factories;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// The <see cref="RegistrationBase"/> that is used to register an Interface
    /// </summary>
    public abstract class RegistrationBase : IRegistrationBase
    {
        private readonly IocContainer _container;

        /// <summary>
        /// The <see cref="RegistrationBase"/> that is used to register an Interface
        /// </summary>
        /// <param name="interfaceType">The <see cref="Type"/> of the Interface</param>
        /// <param name="lifestyle">The <see cref="LightweightIocContainer.Lifestyle"/> of the registration</param>
        /// <param name="container">The current instance of the <see cref="IIocContainer"/></param>
        protected RegistrationBase(Type interfaceType, Lifestyle lifestyle, IocContainer container)
        {
            InterfaceType = interfaceType;
            Lifestyle = lifestyle;
            _container = container;
        }

        /// <summary>
        /// The <see cref="Type"/> of the Interface that is registered with this <see cref="RegistrationBase"/>
        /// </summary>
        public Type InterfaceType { get; }

        /// <summary>
        /// The <see cref="LightweightIocContainer.Lifestyle"/> of Instances that are created with this <see cref="RegistrationBase"/>
        /// </summary>
        public Lifestyle Lifestyle { get; }

        /// <summary>
        /// An <see cref="Array"/> of parameters that are used to <see cref="IIocContainer.Resolve{T}()"/> an instance of this <see cref="IRegistration.InterfaceType"/>
        /// <para>Can be set in the <see cref="IIocInstaller"/> by calling <see cref="WithParameters(object[])"/></para>
        /// </summary>
        public object[] Parameters { get; private set; }
        
        /// <summary>
        /// The Factory added with the <see cref="WithFactory{TFactory}"/> method
        /// </summary>
        public ITypedFactory Factory { get; private set; }

        /// <summary>
        /// Pass parameters that will be used to <see cref="IIocContainer.Resolve{T}()"/> an instance of this <see cref="IRegistration.InterfaceType"/>
        /// <para>Parameters set with this method are always inserted at the beginning of the argument list if more parameters are given when resolving</para>
        /// </summary>
        /// <param name="parameters">The parameters</param>
        /// <returns>The current instance of this <see cref="IRegistration"/></returns>
        /// <exception cref="InvalidRegistrationException"><see cref="Parameters"/> are already set or no parameters given</exception>
        public virtual IRegistrationBase WithParameters(params object[] parameters)
        {
            if (Parameters != null)
                throw new InvalidRegistrationException($"Don't use `WithParameters()` method twice (Type: {InterfaceType}).");

            if (parameters == null || !parameters.Any())
                throw new InvalidRegistrationException($"No parameters given to `WithParameters()` method (Type: {InterfaceType}).");

            Parameters = parameters;
            return this;
        }

        /// <summary>
        /// Pass parameters that will be used to<see cref="IIocContainer.Resolve{T}()"/> an instance of this <see cref="IRegistration.InterfaceType"/>
        /// <para>Parameters set with this method are inserted at the position in the argument list that is passed with the parameter if more parameters are given when resolving</para>
        /// </summary>
        /// <param name="parameters">The parameters with their position</param>
        /// <returns>The current instance of this <see cref="IRegistration"/></returns>
        /// <exception cref="InvalidRegistrationException"><see cref="Parameters"/> are already set or no parameters given</exception>
        public virtual IRegistrationBase WithParameters(params (int index, object parameter)[] parameters)
        {
            if (Parameters != null)
                throw new InvalidRegistrationException($"Don't use `WithParameters()` method twice (Type: {InterfaceType}).");

            if (parameters == null || !parameters.Any())
                throw new InvalidRegistrationException($"No parameters given to `WithParameters()` method (Type: {InterfaceType}).");

            int lastIndex = parameters.Max(p => p.index);
            Parameters = new object[lastIndex + 1];

            for (int i = 0; i < Parameters.Length; i++)
            {
                if (parameters.Any(p => p.index == i))
                    Parameters[i] = parameters.First(p => p.index == i).parameter;
                else
                    Parameters[i] = new InternalResolvePlaceholder();
            }

            return this;
        }

        /// <summary>
        /// Register an abstract typed factory for the <see cref="IRegistrationBase"/> 
        /// </summary>
        /// <typeparam name="TFactory">The type of the abstract typed factory</typeparam>
        /// <returns>The current instance of this <see cref="IRegistrationBase"/></returns>
        public IRegistrationBase WithFactory<TFactory>()
        {
            TypedFactory<TFactory> factory = new(_container);
            
            Factory = factory;
            _container.RegisterFactory(factory);
            
            return this;
        }

        /// <summary>
        /// Register a custom implemented factory for the <see cref="IRegistrationBase"/>
        /// </summary>
        /// <typeparam name="TFactoryInterface">The type of the interface for the custom factory</typeparam>
        /// <typeparam name="TFactoryImplementation">The type of the implementation for the custom factory</typeparam>
        /// <returns>The current instance of this <see cref="IRegistrationBase"/></returns>
        public IRegistrationBase WithFactory<TFactoryInterface, TFactoryImplementation>() where TFactoryImplementation : TFactoryInterface
        {
            Factory = new CustomTypedFactory<TFactoryInterface>();
            _container.Register<TFactoryInterface, TFactoryImplementation>();

            return this;
        }
    }
}