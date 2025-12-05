// Author: Simon.Gockner
// Created: 2025-12-01
// Copyright(c) 2025 SimonG. All Rights Reserved.

using Debug.LightweightIocContainer.FactoryGenerator.Samples;
using Debug.LightweightIocContainer.FactoryGenerator.Samples.Factories;
using Debug.LightweightIocContainer.FactoryGenerator.Samples.Interfaces;
using LightweightIocContainer.FactoryGenerator;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;

namespace Debug.LightweightIocContainer.FactoryGenerator;

public class SampleInstaller : IIocInstaller
{
    public void Install(IRegistrationCollector registration)
    {
        registration.Add<ISampleClass, SampleClass>().WithGeneratedFactory<ISampleClassFactory>();
        registration.Add<IOtherSample, OtherSample>().WithGeneratedFactory<IGenericSampleFactory<IOtherSample>>();
        registration.Add<IOtherSample2, OtherSample2>().WithGeneratedFactory<IGenericSampleFactory<IOtherSample2>>();
    }
}