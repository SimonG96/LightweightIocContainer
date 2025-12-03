using Debug.LightweightIocContainer.FactoryGenerator.Samples.Interfaces;

namespace Debug.LightweightIocContainer.FactoryGenerator.Samples.Factories;

public interface ISampleClassFactory
{
    ISampleClass Create();
    T Create<T>() where T : ISampleClass;
    Task<ISampleClass> CreateAsync();
    
    ISampleClass Create(string name);
    T Create<T, U, V>(U param1, V param2) where T : ISampleClass where U : class, new() where V : struct;
    Task<T> CreateAsync<T, U>(U parameter) where T : ISampleClass where U : class;
    
    ISampleClass Create<T>(Task<T> task) where T : Task<ISampleClass>;
    
    Task<T?> Create<T>(ISampleClass? sampleClass) where T : class, ISampleClass;
    ISampleClass? Create(int id);
    
    void ClearMultitonInstance<T>() where T : ISampleClass;
}