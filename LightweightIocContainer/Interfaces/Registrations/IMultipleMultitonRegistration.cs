// Author: Gockner, Simon
// Created: 2020-11-19
// Copyright(c) 2020 SimonG. All Rights Reserved.

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// An <see cref="IRegistration"/> to register multiple interfaces for on implementation type that implements them as a multiton
    /// </summary>
    /// <typeparam name="TInterface1">The first interface</typeparam>
    /// <typeparam name="TInterface2">The second interface</typeparam>
    /// <typeparam name="TImplementation">The implementation</typeparam>
    public interface IMultipleMultitonRegistration<TInterface1, TInterface2, TImplementation> : IMultitonRegistration<TInterface1, TImplementation>, IMultipleRegistration<TInterface1, TInterface2, TImplementation> where TImplementation : TInterface1, TInterface2
    {
        
    }
}