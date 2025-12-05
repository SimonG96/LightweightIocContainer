// Author: Simon.Gockner
// Created: 2025-12-05
// Copyright(c) 2025 SimonG. All Rights Reserved.

using Debug.LightweightIocContainer.FactoryGenerator.Samples.Interfaces;

namespace Debug.LightweightIocContainer.FactoryGenerator.Samples.Factories;

public interface IGenericSampleFactory<T> where T : IOtherSample
{
    T Create();
}