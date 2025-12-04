namespace Test.LightweightIocContainer.FactoryGenerator.TestClasses;

public class AsyncMultitonTest(int id) : AsyncTest
{
    public int Id { get; } = id;
    public override async Task Initialize()
    {
        if (IsInitialized)
            throw new Exception();
            
        await base.Initialize();
    }
}