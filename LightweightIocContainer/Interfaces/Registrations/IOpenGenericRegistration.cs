// Author: Simon Gockner
// Created: 2020-09-18
// Copyright(c) 2020 SimonG. All Rights Reserved.

using System;

namespace LightweightIocContainer.Interfaces.Registrations
{
    public interface IOpenGenericRegistration : IRegistration, ILifestyleProvider
    {
        Type ImplementationType { get; }
    }
}