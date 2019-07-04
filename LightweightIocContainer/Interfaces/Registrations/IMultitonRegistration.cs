// Author: Gockner, Simon
// Created: 2019-06-07
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// The registration that is used to register a multiton
    /// </summary>
    /// <typeparam name="TInterface">The registered interface</typeparam>
    public interface IMultitonRegistration<TInterface> : IDefaultRegistration<TInterface>
    {
        /// <summary>
        /// The <see cref="Type"/> of the multiton scope
        /// </summary>
        Type Scope { get; }
    }
}