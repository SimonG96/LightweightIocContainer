// Author: Simon Gockner
// Created: 2019-12-07
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System.Collections.Generic;

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// The base interface for every <see cref="IMultipleRegistration{TInterface1,TInterface2,TImplementation}"/> to register multiple interfaces
    /// </summary>
    /// <typeparam name="TInterface1">The first interface</typeparam>
    /// <typeparam name="TImplementation">The implementation</typeparam>
    public interface IMultipleRegistration<TInterface1, TImplementation> : ITypedRegistrationBase<TInterface1, TImplementation> where TImplementation : TInterface1
    {
        /// <summary>
        /// A <see cref="List{T}"/> of <see cref="IRegistration"/>s that are registered within this <see cref="IMultipleRegistration{TInterface1,TImplementation}"/>
        /// </summary>
        List<IRegistration> Registrations { get; }
    }

    /// <summary>
    /// An <see cref="IRegistrationBase{TInterface}"/> to register multiple interfaces for on implementation type
    /// </summary>
    /// <typeparam name="TInterface1">The first interface</typeparam>
    /// <typeparam name="TInterface2">The second interface</typeparam>
    /// <typeparam name="TImplementation">The implementation</typeparam>
    public interface IMultipleRegistration<TInterface1, TInterface2, TImplementation> : IMultipleRegistration<TInterface1, TImplementation> where TImplementation : TInterface1, TInterface2
    {
        
    }

    /// <summary>
    /// An <see cref="IRegistrationBase{TInterface}"/> to register multiple interfaces for on implementation type
    /// </summary>
    /// <typeparam name="TInterface1">The first interface</typeparam>
    /// <typeparam name="TInterface2">The second interface</typeparam>
    /// <typeparam name="TInterface3">The third interface</typeparam>
    /// <typeparam name="TImplementation">The implementation</typeparam>
    public interface IMultipleRegistration<TInterface1, TInterface2, TInterface3, TImplementation> : IMultipleRegistration<TInterface1, TImplementation> where TImplementation : TInterface1, TInterface2, TInterface3
    {

    }

    /// <summary>
    /// An <see cref="IRegistrationBase{TInterface}"/> to register multiple interfaces for on implementation type
    /// </summary>
    /// <typeparam name="TInterface1">The first interface</typeparam>
    /// <typeparam name="TInterface2">The second interface</typeparam>
    /// <typeparam name="TInterface3">The third interface</typeparam>
    /// <typeparam name="TInterface4">The fourth interface</typeparam>
    /// <typeparam name="TImplementation">The implementation</typeparam>
    public interface IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TImplementation> : IMultipleRegistration<TInterface1, TImplementation> where TImplementation : TInterface1, TInterface2, TInterface3, TInterface4
    {

    }

    /// <summary>
    /// An <see cref="IRegistrationBase{TInterface}"/> to register multiple interfaces for on implementation type
    /// </summary>
    /// <typeparam name="TInterface1">The first interface</typeparam>
    /// <typeparam name="TInterface2">The second interface</typeparam>
    /// <typeparam name="TInterface3">The third interface</typeparam>
    /// <typeparam name="TInterface4">The fourth interface</typeparam>
    /// <typeparam name="TInterface5">The fifth interface</typeparam>
    /// <typeparam name="TImplementation">The implementation</typeparam>
    public interface IMultipleRegistration<TInterface1, TInterface2, TInterface3, TInterface4, TInterface5, TImplementation> : IMultipleRegistration<TInterface1, TImplementation> where TImplementation : TInterface1, TInterface2, TInterface3, TInterface4, TInterface5
    {

    }
}