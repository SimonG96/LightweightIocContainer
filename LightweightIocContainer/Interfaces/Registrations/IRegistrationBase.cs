// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces.Registrations.Fluent;

namespace LightweightIocContainer.Interfaces.Registrations;

/// <summary>
/// The <see cref="IRegistrationBase"/> that is used to register an Interface and extends the <see cref="IRegistration"/> with fluent options
/// </summary>
public interface IRegistrationBase : IRegistration, IWithFactory, IWithParameters, IWithDisposeStrategy
{
        
}