namespace Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

public interface IAsyncTest
{
    bool IsInitialized { get; }
    Task Initialize();
}