using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

namespace Test.LightweightIocContainer.FactoryGenerator.TestClasses.Factories;

public interface IGenericTestFactory
{
    IGenericTest<T> Create<T>() where T : IConstraint, new();
}