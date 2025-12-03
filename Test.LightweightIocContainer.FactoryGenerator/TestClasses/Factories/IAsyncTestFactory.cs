using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

namespace Test.LightweightIocContainer.FactoryGenerator.TestClasses.Factories;

public interface IAsyncTestFactory
{
    Task<IAsyncTest> Create();
}