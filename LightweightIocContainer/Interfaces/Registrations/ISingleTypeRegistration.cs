// Author: Gockner, Simon
// Created: 2019-11-22
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// The <see cref="IRegistrationBase{TInterface}"/> to register either only an interface or only a <see cref="Type"/>
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the <see cref="IRegistrationBase{TInterface}"/></typeparam>
    public interface ISingleTypeRegistration<T> : IRegistrationBase<T>
    {
        /// <summary>
        /// <see cref="Func{T,TResult}"/> that is invoked instead of creating an instance of this <see cref="Type"/> the default way
        /// </summary>
        Func<IIocContainer, T> FactoryMethod { get; }

        /// <summary>
        /// Pass a <see cref="Func{T,TResult}"/> that will be invoked instead of creating an instance of this <see cref="Type"/> the default way
        /// </summary>
        /// <param name="factoryMethod">The <see cref="Func{T,TResult}"/></param>
        /// <returns>The current instance of this <see cref="IRegistrationBase{TInterface}"/></returns>
        IRegistrationBase<T> WithFactoryMethod(Func<IIocContainer, T> factoryMethod);
    }
}