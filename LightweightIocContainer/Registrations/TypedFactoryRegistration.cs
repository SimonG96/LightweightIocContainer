// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Factories;
using LightweightIocContainer.Interfaces.Registrations;

namespace LightweightIocContainer.Registrations
{
    /// <summary>
    /// The registration that is used to register an abstract typed factory
    /// </summary>
    /// <typeparam name="TFactory">The <see cref="Type"/> of the abstract typed factory</typeparam>
    public class TypedFactoryRegistration<TFactory> : ITypedFactoryRegistration<TFactory>
    {
        /// <summary>
        /// The registration that is used to register an abstract typed factory
        /// </summary>
        /// <param name="factory">The <see cref="ITypedFactory{TFactory}"/> for this <see cref="IRegistration"/></param>
        public TypedFactoryRegistration(ITypedFactory<TFactory> factory) => Factory = factory;

        /// <summary>
        /// The <see cref="Type"/> of the factory that is registered with this <see cref="IRegistration"/>
        /// </summary>
        public Type InterfaceType => typeof(TFactory);
        
        /// <summary>
        /// The class that contains the implemented abstract factory of this <see cref="TypedFactoryRegistration{TFactory}"/>
        /// </summary>
        public ITypedFactory<TFactory> Factory { get; }
    }
}