using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

namespace Test.LightweightIocContainer.FactoryGenerator.TestClasses;

public class AsyncTest : IAsyncTest
{
    public bool IsInitialized { get; private set; }

    public virtual async Task Initialize()
    {
        await Task.Delay(200);
        IsInitialized = true;
    }
}