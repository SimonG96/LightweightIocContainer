using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

namespace Test.LightweightIocContainer.FactoryGenerator.TestClasses.Factories;

public interface ICtorGenericTestFactory
{
    IGenericTest<T> Create<T>(T item) where T : IConstraint, new();
}