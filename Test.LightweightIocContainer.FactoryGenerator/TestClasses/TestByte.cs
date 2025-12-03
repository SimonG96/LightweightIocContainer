using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

namespace Test.LightweightIocContainer.FactoryGenerator.TestClasses;

public class TestByte : IDefaultTest
{
    private readonly byte _id;

    public TestByte(byte id) => _id = id;
}