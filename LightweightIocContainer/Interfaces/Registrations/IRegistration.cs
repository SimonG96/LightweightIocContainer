// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Interfaces.Registrations;

/// <summary>
/// The base registration that is used to register an Interface
/// </summary>
public interface IRegistration
{
    /// <summary>
    /// The <see cref="Type"/> of the Interface that is registered with this <see cref="IRegistration"/>
    /// </summary>
    Type InterfaceType { get; }
}