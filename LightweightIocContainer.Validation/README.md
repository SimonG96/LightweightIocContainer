## Get started with the Lightweight IOC Container Validator

### How to install

The easiest way to [install](https://github.com/SimonG96/LghtweightIocContainer/wiki/Install-Lightweight-IOC-Container) the Lightweight IOC Container is by using [NuGet](https://www.nuget.org/packages/LightweightIocContainer.Validation/) through the [`.NET CLI`](https://github.com/SimonG96/LightweightIocContainer/wiki/Install-Lightweight-IOC-Container#net-cli):

```.net
> dotnet add package LightweightIocContainer.Validaton --version 4.0.0-beta7
```

### Validation

You can validate your `IocContainer` setup by using the `IocValidator` in a unit test:

```c#
[TestFixture]
public class IocValidationTest
{
    [Test]
    public void ValidateIocContainerSetup()
    {
        IocContainer container = new();
        container.Install(new Installer());

        IocValidator validator = new(container);
        validator.Validate();
    }
}
```

If this test is successful, everything is correctly installed and can be resolved by the `IocContainer`. By going through the thrown exceptions in case of a failed test you will see what is not working correctly with your current setup.
