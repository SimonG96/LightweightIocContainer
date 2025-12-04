using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Factories;
using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

namespace Test.LightweightIocContainer.FactoryGenerator.TestClasses;

public class TestNull : IDefaultTest
{

    public TestNull(object obj, string content, string optional1, string optional2, ITestNullFactory testNullFactory)
    {
        Obj = obj;
        Content = content;
        Optional1 = optional1;
        Optional2 = optional2;
        TestNullFactory = testNullFactory;
    }
        
    public object Obj { get; }
    public string Content { get; }
    public string Optional1 { get; }
    public string Optional2 { get; }
    public ITestNullFactory TestNullFactory { get; }
}