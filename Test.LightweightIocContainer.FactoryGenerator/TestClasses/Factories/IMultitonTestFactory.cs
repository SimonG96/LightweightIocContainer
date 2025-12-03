using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

namespace Test.LightweightIocContainer.FactoryGenerator.TestClasses.Factories;

public interface IMultitonTestFactory
{
    IDefaultTest Create(MultitonScope scope);
    void ClearMultitonInstance<T>();
}