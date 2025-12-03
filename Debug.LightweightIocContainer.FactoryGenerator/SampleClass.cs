// Author: Simon.Gockner
// Created: 2025-12-01
// Copyright(c) 2025 SimonG. All Rights Reserved.

namespace Debug.LightweightIocContainer.FactoryGenerator;

public class SampleClass : ISampleClass;
public interface ISampleClass;

public interface ISampleClassFactory
{
    ISampleClass Create();
    T Create<T>() where T : ISampleClass;
    Task<ISampleClass> CreateAsync();
    
    ISampleClass Create(string name);
    T Create<T, U, V>(U param1, V param2) where T : ISampleClass where U : class, new() where V : struct;
    Task<T> CreateAsync<T, U>(U parameter) where T : ISampleClass where U : class;
    
    void ClearMultitonInstance<T>() where T : ISampleClass;
}