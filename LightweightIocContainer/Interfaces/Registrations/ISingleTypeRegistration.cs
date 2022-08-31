// Author: Gockner, Simon
// Created: 2019-11-22
// Copyright(c) 2019 SimonG. All Rights Reserved.

namespace LightweightIocContainer.Interfaces.Registrations;

/// <summary>
/// The <see cref="IRegistration"/> to register either only an interface or only a <see cref="Type"/>
/// </summary>
/// <typeparam name="T">The <see cref="Type"/> of the <see cref="IRegistration"/></typeparam>
public interface ISingleTypeRegistration<T> : IRegistrationBase
{
    /// <summary>
    /// <see cref="Func{T,TResult}"/> that is invoked instead of creating an instance of this <see cref="Type"/> the default way
    /// </summary>
    Func<IIocResolver, T>? FactoryMethod { get; }

    /// <summary>
    /// Pass a <see cref="Func{T,TResult}"/> that will be invoked instead of creating an instance of this <see cref="Type"/> the default way
    /// </summary>
    /// <param name="factoryMethod">The <see cref="Func{T,TResult}"/></param>
    /// <returns>The current instance of this <see cref="IRegistration"/></returns>
    ISingleTypeRegistration<T> WithFactoryMethod(Func<IIocResolver, T> factoryMethod);
}