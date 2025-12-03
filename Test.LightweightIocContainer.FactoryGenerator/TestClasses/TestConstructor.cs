using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

namespace Test.LightweightIocContainer.FactoryGenerator.TestClasses;

public class TestConstructor : IDefaultTest
{
    public TestConstructor(string name, DefaultTest test)
    {

    }

    public TestConstructor(DefaultTest test, string name = null!)
    {

    }
}