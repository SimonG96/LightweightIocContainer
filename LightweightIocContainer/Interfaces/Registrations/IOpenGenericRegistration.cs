// Author: Simon Gockner
// Created: 2020-09-18
// Copyright(c) 2020 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// <see cref="IRegistration"/> for open generic types
    /// </summary>
    public interface IOpenGenericRegistration : IRegistration, ILifestyleProvider
    {
        /// <summary>
        /// The <see cref="Type"/> that implements the <see cref="IRegistration.InterfaceType"/> that is registered with this <see cref="IOpenGenericRegistration"/>
        /// </summary>
        Type ImplementationType { get; }
    }
}