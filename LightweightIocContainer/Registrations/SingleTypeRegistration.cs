// Author: Gockner, Simon
// Created: 2019-11-22
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// The <see cref="IRegistrationBase{TInterface}"/> to register either only an interface or only a <see cref="Type"/>
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the <see cref="IRegistrationBase{TInterface}"/></typeparam>
    public class SingleTypeRegistration<T> : RegistrationBase<T>, ISingleTypeRegistration<T>
    {
        /// <summary>
        /// The <see cref="IRegistrationBase{TInterface}"/> to register either only an interface or only a <see cref="Type"/>
        /// </summary>
        /// <param name="interfaceType">The <see cref="Type"/> of the interface or <see cref="Type"/></param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> of the <see cref="RegistrationBase{TInterface}"/></param>
        public SingleTypeRegistration(Type interfaceType, Lifestyle lifestyle)
            : base(interfaceType, lifestyle)
        {
            Name = $"{InterfaceType.Name}, Lifestyle: {Lifestyle.ToString()}";
        }

        /// <summary>
        /// <see cref="Func{T,TResult}"/> that is invoked instead of creating an instance of this <see cref="Type"/> the default way
        /// </summary>
        public Func<IIocContainer, T> FactoryMethod { get; private set; }

        /// <summary>
        /// Pass a <see cref="Func{T,TResult}"/> that will be invoked instead of creating an instance of this <see cref="Type"/> the default way
        /// </summary>
        /// <param name="factoryMethod">The <see cref="Func{T,TResult}"/></param>
        /// <returns>The current instance of this <see cref="IRegistrationBase{TInterface}"/></returns>
        public ISingleTypeRegistration<T> WithFactoryMethod(Func<IIocContainer, T> factoryMethod)
        {
            FactoryMethod = factoryMethod;
            return this;
        }
    }
}