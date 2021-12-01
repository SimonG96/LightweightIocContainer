// Author: simon.gockner
// Created: 2019-05-20
// Copyright(c) 2019 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces.Registrations.Fluent;

namespace LightweightIocContainer.Interfaces.Registrations
{
    public interface IRegistrationBase : IRegistration, IWithFactory, IWithParameters, ILifestyleProvider
    {
        
    }
}