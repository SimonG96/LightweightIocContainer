# Lightweight IOC Container

A lightweight IOC Container that is powerful enough to do all the things you need it to do.  

[![GitHub Actions](https://github.com/SimonG96/LightweightIocContainer/workflows/CI/badge.svg)](https://github.com/SimonG96/LightweightIocContainer/actions)

[![Nuget](https://img.shields.io/nuget/dt/LightweightIocContainer.svg?label=IocContainer%20NuGet%20Downloads&logo=NuGet)](https://www.nuget.org/packages/LightweightIocContainer/)
[![Nuget](https://img.shields.io/nuget/v/LightweightIocContainer.svg?label=IocContainer%20NuGet%20Version&logo=NuGet)](https://www.nuget.org/packages/LightweightIocContainer/)
[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/LightweightIocContainer.svg?label=IocContainer%20NuGet%20Pre-Release&logo=NuGet)](https://www.nuget.org/packages/LightweightIocContainer/)

[![Nuget](https://img.shields.io/nuget/dt/LightweightIocContainer.Validation.svg?label=Validation%20NuGet%20Downloads&logo=NuGet)](https://www.nuget.org/packages/LightweightIocContainer.Validation/)
[![Nuget](https://img.shields.io/nuget/v/LightweightIocContainer.Validation.svg?label=Validation%20NuGet%20Version&logo=NuGet)](https://www.nuget.org/packages/LightweightIocContainer.Validation/)
[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/LightweightIocContainer.Validation.svg?label=Validation%20NuGet%20Pre-Release&logo=NuGet)](https://www.nuget.org/packages/LightweightIocContainer.Validation/)

## Get started with the Lightweight IOC Container

### How to install

The easiest way to [install](https://github.com/SimonG96/LightweightIocContainer/wiki/Install-Lightweight-IOC-Container) the Lightweight IOC Container is by using [NuGet](https://www.nuget.org/packages/LightweightIocContainer/) through the [`.NET CLI`](https://github.com/SimonG96/LightweightIocContainer/wiki/Install-Lightweight-IOC-Container#net-cli):

```.net
> dotnet add package LightweightIocContainer --version 4.4.0-beta2
```

### Example usage

  1. [Instantiate `IocContainer`](https://github.com/SimonG96/LightweightIocContainer/wiki/Simple-Usage-of-Lightweight-IOC-Container#instantiate-container):
  
      ```c#
      IocContainer container = new();
      ```

  2. Install [`IIocInstaller`s](https://github.com/SimonG96/LightweightIocContainer/wiki/IIocInstaller) for the container:

      ```c#
      container.Install(new Installer());
      ```

  3. [Resolve](https://github.com/SimonG96/LightweightIocContainer/wiki/Simple-Usage-of-Lightweight-IOC-Container#resolving-instances) one instance from the container:

      ```c#
      IFooFactory fooFactory = container.Resolve<IFooFactory>();
      ```

  4. Use this instance to [create](https://github.com/SimonG96/LightweightIocContainer/wiki/Advanced-Usage-of-Lightweight-IOC-Container#use-factories-to-resolve-instances) what your application needs:

      ```c#
      IFoo foo = fooFactory.Create();
      ```

  5. When your application is finished, don't forget to [dispose](https://github.com/SimonG96/LightweightIocContainer/wiki/Simple-Usage-of-Lightweight-IOC-Container#Disposing-Container) your `IocContainer`:

      ```c#
      container.Dispose();
      ```

### Validation

There is the option to install the [LightweightIocContainer.Validation](https://www.nuget.org/packages/LightweightIocContainer.Validation/) package:

```.net
> dotnet add package LightweightIocContainer.Validaton --version 4.4.0-beta2
```

With this you can validate your `IocContainer` setup by using the `IocValidator` in a unit test:

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

### Demo Project

There is a [demo project][demoProjectLink] available where you can check out how different functions of the Lightweight IOC Container can be used.

[demoProjectLink]: https://github.com/SimonG96/LightweightIocContainer_Example
