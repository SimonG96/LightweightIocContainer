// Author: Simon.Gockner
// Created: 2025-12-01
// Copyright(c) 2025 SimonG. All Rights Reserved.

using LightweightIocContainer.FactoryGenerator;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;

namespace Debug.LightweightIocContainer.FactoryGenerator;

public class SampleInstaller : IIocInstaller
{
    public void Install(IRegistrationCollector registration)
    {
        registration.Add<ISampleClass, SampleClass>().WithGeneratedFactory<ISampleClassFactory>();
    }
}