// Author: Simon.Gockner
// Created: 2025-12-01
// Copyright(c) 2025 SimonG. All Rights Reserved.

namespace Debug.LightweightIocContainer.FactoryGenerator;

public class SampleClass : ISampleClass;
public interface ISampleClass;

public interface ISampleClassFactory
{
    ISampleClass Create();
}