## Get started with the Lightweight IOC Container Factory Generator

### How to install

The easiest way to [install](https://github.com/SimonG96/LghtweightIocContainer/wiki/Install-Lightweight-IOC-Container) the Lightweight IOC Container is by using [NuGet](https://www.nuget.org/packages/LightweightIocContainer.FactoryGenerator/) through the [`.NET CLI`](https://github.com/SimonG96/LightweightIocContainer/wiki/Install-Lightweight-IOC-Container#net-cli):

```.net
> dotnet add package LightweightIocContainer.FactoryGenerator --version 5.0.0
```

### Example usage

When registering an interface you can use the `WithGeneratedFactory<>()` method to generate a factory for the registered type:

```c#
public class SampleInstaller : IIocInstaller
{
    public void Install(IRegistrationCollector registration)
    {
        registration.Add<ISampleClass, SampleClass>().WithGeneratedFactory<ISampleClassFactory>();
    }
}
```
