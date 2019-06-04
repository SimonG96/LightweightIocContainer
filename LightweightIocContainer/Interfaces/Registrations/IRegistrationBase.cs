// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Interfaces.Registrations
{
    /// <summary>
    /// The base registration that is used to register an Interface
    /// </summary>
    public interface IRegistrationBase
    {
        /// <summary>
        /// The name of the <see cref="IRegistrationBase"/>
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The Type of the Interface that is registered with this <see cref="IRegistrationBase"/>
        /// </summary>
        Type InterfaceType { get; }
    }
}