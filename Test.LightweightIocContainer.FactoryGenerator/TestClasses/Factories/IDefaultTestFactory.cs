using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

namespace Test.LightweightIocContainer.FactoryGenerator.TestClasses.Factories;

public interface IDefaultTestFactory
{
    IDefaultTest Create();
    IDefaultTest Create(string name);
    IDefaultTest CreateTest(string name = null!);
    IDefaultTest Create(byte id);
}