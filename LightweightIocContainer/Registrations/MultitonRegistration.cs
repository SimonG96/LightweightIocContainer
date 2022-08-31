// Author: Gockner, Simon
// Created: 2019-06-07
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Linq;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations;

/// <summary>
/// The registration that is used to register a multiton
/// </summary>
/// <typeparam name="TInterface">The registered interface</typeparam>
/// <typeparam name="TImplementation">The registered implementation</typeparam>
internal class MultitonRegistration<TInterface, TImplementation> : TypedRegistration<TInterface, TImplementation>, IMultitonRegistration<TInterface, TImplementation> where TImplementation : TInterface
{
    /// <summary>
    /// The registration that is used to register a multiton
    /// </summary>
    /// <param name="interfaceType">The <see cref="Type"/> of the Interface</param>
    /// <param name="implementationType">The <see cref="Type"/> of the Implementation</param>
    /// <param name="scope">The <see cref="Type"/> of the Multiton Scope</param>
    /// <param name="container">The current instance of the <see cref="IIocContainer"/></param>
    public MultitonRegistration(Type interfaceType, Type implementationType, Type scope, IocContainer container)
        : base(interfaceType, implementationType, Lifestyle.Multiton, container) =>
        Scope = scope;

    /// <summary>
    /// The <see cref="Type"/> of the multiton scope
    /// </summary>
    public Type Scope { get; }

    /// <summary>
    /// Validate the <see cref="RegistrationBase.Factory"/>
    /// </summary>
    protected override void ValidateFactory()
    {
        if (Factory == null)
            return;
            
        if (Factory.CreateMethods.Any(c => c.GetParameters().Length == 0))
            throw new InvalidFactoryRegistrationException($"Create methods without parameters are not valid for multitons (Type: {InterfaceType}).");
            
        if (Factory.CreateMethods.Any(c => c.GetParameters()[0].ParameterType != Scope))
            throw new InvalidFactoryRegistrationException($"Create methods without scope type ({Scope}) as first parameter are not valid for multitons (Type: {InterfaceType}).");
            
        base.ValidateFactory();
    }

    public override bool Equals(object? obj) => obj is MultitonRegistration<TInterface, TImplementation> multitonRegistration && 
                                                base.Equals(obj) &&
                                                Scope == multitonRegistration.Scope;

    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Scope);
}