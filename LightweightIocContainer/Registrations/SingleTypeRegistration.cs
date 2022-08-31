// Author: Gockner, Simon
// Created: 2019-11-22
// Copyright(c) 2019 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations;

/// <summary>
/// The <see cref="IRegistration"/> to register either only an interface or only a <see cref="Type"/>
/// </summary>
/// <typeparam name="T">The <see cref="Type"/> of the <see cref="IRegistration"/></typeparam>
internal class SingleTypeRegistration<T> : RegistrationBase, ISingleTypeRegistration<T>
{
    /// <summary>
    /// The <see cref="IRegistration"/> to register either only an interface or only a <see cref="Type"/>
    /// </summary>
    /// <param name="interfaceType">The <see cref="Type"/> of the interface or <see cref="Type"/></param>
    /// <param name="lifestyle">The <see cref="Lifestyle"/> of the <see cref="RegistrationBase"/></param>
    /// <param name="container">The current instance of the <see cref="IIocContainer"/></param>
    public SingleTypeRegistration(Type interfaceType, Lifestyle lifestyle, IocContainer container)
        : base(interfaceType, lifestyle, container)
    {
            
    }

    /// <summary>
    /// <see cref="Func{T,TResult}"/> that is invoked instead of creating an instance of this <see cref="Type"/> the default way
    /// </summary>
    public Func<IIocResolver, T>? FactoryMethod { get; private set; }

    /// <summary>
    /// Pass a <see cref="Func{T,TResult}"/> that will be invoked instead of creating an instance of this <see cref="Type"/> the default way
    /// </summary>
    /// <param name="factoryMethod">The <see cref="Func{T,TResult}"/></param>
    /// <returns>The current instance of this <see cref="IRegistration"/></returns>
    public ISingleTypeRegistration<T> WithFactoryMethod(Func<IIocResolver, T> factoryMethod)
    {
        FactoryMethod = factoryMethod;
        return this;
    }
        
    public override bool Equals(object? obj)
    {
        if (obj is not SingleTypeRegistration<T> singleTypeRegistration)
            return false;

        if (FactoryMethod == null && singleTypeRegistration.FactoryMethod != null)
            return false;

        if (FactoryMethod != null && singleTypeRegistration.FactoryMethod == null)
            return false;
            
        return base.Equals(obj);
    }

    public override int GetHashCode() => base.GetHashCode();
}