using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

namespace Test.LightweightIocContainer.FactoryGenerator.TestClasses.Factories;

public interface IAsyncMultitonTestFactory
{
    Task<IAsyncTest> Create(int id);
}