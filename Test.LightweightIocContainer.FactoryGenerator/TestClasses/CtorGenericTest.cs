using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

namespace Test.LightweightIocContainer.FactoryGenerator.TestClasses;

public class CtorGenericTest<T> : IGenericTest<T> where T : IConstraint, new()
{
    public CtorGenericTest(T item)
    {
            
    }
}