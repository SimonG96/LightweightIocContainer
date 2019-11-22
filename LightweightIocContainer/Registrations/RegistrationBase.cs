// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Linq;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// The <see cref="RegistrationBase{TInterface}"/> that is used to register an Interface
    /// </summary>
    /// <typeparam name="TInterface">The registered Interface</typeparam>
    public abstract class RegistrationBase<TInterface> : IRegistrationBase<TInterface>
    {
        /// <summary>
        /// The <see cref="RegistrationBase{TInterface}"/> that is used to register an Interface
        /// </summary>
        /// <param name="interfaceType">The <see cref="Type"/> of the Interface</param>
        /// <param name="lifestyle">The <see cref="LightweightIocContainer.Lifestyle"/> of the registration</param>
        protected RegistrationBase(Type interfaceType, Lifestyle lifestyle)
        {
            InterfaceType = interfaceType;
            Lifestyle = lifestyle;
        }

        /// <summary>
        /// The name of the <see cref="RegistrationBase{TInterface}"/>
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The <see cref="Type"/> of the Interface that is registered with this <see cref="RegistrationBase{TInterface}"/>
        /// </summary>
        public Type InterfaceType { get; }

        /// <summary>
        /// The <see cref="LightweightIocContainer.Lifestyle"/> of Instances that are created with this <see cref="RegistrationBase{TInterface}"/>
        /// </summary>
        public Lifestyle Lifestyle { get; }


        /// <summary>
        /// This <see cref="Action{T}"/> is invoked when an instance of this type is created.
        /// <para>Can be set in the <see cref="IIocInstaller"/> by calling <see cref="OnCreate"/></para>
        /// </summary>
        public Action<TInterface> OnCreateAction { get; private set; }


        /// <summary>
        /// Pass an <see cref="Action{T}"/> that will be invoked when an instance of this <see cref="Type"/> is created
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/></param>
        /// <returns>The current instance of this <see cref="IRegistrationBase{TInterface}"/></returns>
        public IRegistrationBase<TInterface> OnCreate(Action<TInterface> action)
        {
            OnCreateAction = action;
            return this;
        }

        /// <summary>
        /// An <see cref="Array"/> of parameters that are used to <see cref="IIocContainer.Resolve{T}()"/> an instance of this <see cref="IRegistration.InterfaceType"/>
        /// <para>Can be set in the <see cref="IIocInstaller"/> by calling <see cref="WithParameters(object[])"/></para>
        /// </summary>
        public object[] Parameters { get; private set; }

        /// <summary>
        /// Pass parameters that will be used to<see cref="IIocContainer.Resolve{T}()"/> an instance of this <see cref="IRegistration.InterfaceType"/>
        /// <para>Parameters set with this method are always inserted at the beginning of the argument list if more parameters are given when resolving</para>
        /// </summary>
        /// <param name="parameters">The parameters</param>
        /// <returns>The current instance of this <see cref="IRegistrationBase{TInterface}"/></returns>
        /// <exception cref="InvalidRegistrationException"><see cref="Parameters"/> are already set or no parameters given</exception>
        public IRegistrationBase<TInterface> WithParameters(params object[] parameters)
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
        /// <returns>The current instance of this <see cref="IRegistrationBase{TInterface}"/></returns>
        /// <exception cref="InvalidRegistrationException"><see cref="Parameters"/> are already set or no parameters given</exception>
        public IRegistrationBase<TInterface> WithParameters(params (int index, object parameter)[] parameters)
        {
            if (Parameters != null)
                throw new InvalidRegistrationException($"Don't use `WithParameters()` method twice (Type: {InterfaceType}).");

            if (parameters == null || !parameters.Any())
                throw new InvalidRegistrationException($"No parameters given to `WithParameters()` method (Type: {InterfaceType}).");

            var lastIndex = parameters.Max(p => p.index);
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
    }
}