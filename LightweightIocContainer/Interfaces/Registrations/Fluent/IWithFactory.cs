// Author: Gockner, Simon
// Created: 2021-11-30
// Copyright(c) 2021 SimonG. All Rights Reserved.

using LightweightIocContainer.Interfaces.Factories;

namespace LightweightIocContainer.Interfaces.Registrations.Fluent
{
    public interface IWithFactory
    {
        ITypedFactory Factory { get; }
        
        IRegistrationBase WithFactory<TFactory>();
        IRegistrationBase WithFactory<TFactoryInterface, TFactoryImplementation>() where TFactoryImplementation : TFactoryInterface;
    }
}