using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

namespace Test.LightweightIocContainer.FactoryGenerator.TestClasses.Factories;

public interface ITestDefaultFactory
{
    IDefaultTest Create(object obj, string content, string optional1, string optional2, ITestNullFactory testNullFactory = null!);
}