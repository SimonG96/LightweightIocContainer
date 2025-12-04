using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Factories;
using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

namespace Test.LightweightIocContainer.FactoryGenerator.TestClasses;

public class A : IA
{
    public A(IBFactory bFactory) => BProperty = bFactory.Create(new C("from A"));
    public IB BProperty { get; }
}