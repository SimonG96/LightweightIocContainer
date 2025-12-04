using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

namespace Test.LightweightIocContainer.FactoryGenerator.TestClasses.Factories;

public interface IInvalidMultitonTestFactory
{
    IDefaultTest Create(MultitonScope scope);
    IDefaultTest Create(int number);
}