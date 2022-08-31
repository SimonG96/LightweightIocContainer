# Lightweight IOC Container

A lightweight IOC Container that is powerful enough to do all the things you need it to do.  

[![GitHub Actions](https://github.com/SimonG96/LightweightIocContainer/workflows/CI/badge.svg)](https://github.com/SimonG96/LightweightIocContainer/actions)

[![Nuget](https://img.shields.io/nuget/dt/LightweightIocContainer.svg?label=NuGet%20Downloads&logo=NuGet)](https://www.nuget.org/packages/LightweightIocContainer/)
[![Nuget](https://img.shields.io/nuget/v/LightweightIocContainer.svg?label=NuGet%20Version&logo=NuGet)](https://www.nuget.org/packages/LightweightIocContainer/)
[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/LightweightIocContainer.svg?label=NuGet%20Pre-Release&logo=NuGet)](https://www.nuget.org/packages/LightweightIocContainer/)

## Get started with the Lightweight IOC Container Validator

### How to install

The easiest way to [install](https://github.com/SimonG96/LightweightIocContainer/wiki/Install-Lightweight-IOC-Container) the Lightweight IOC Container is by using [NuGet](https://www.nuget.org/packages/LightweightIocContainer/).  
You can either use the [`PackageManager`](https://github.com/SimonG96/LightweightIocContainer/wiki/Install-Lightweight-IOC-Container#packagemanager) in VisualStudio:

```PM
PM> Install-Package LightweightIocContainer.Validator -Version 4.0.0-beta4
```

or you can use the [`.NET CLI`](https://github.com/SimonG96/LightweightIocContainer/wiki/Install-Lightweight-IOC-Container#net-cli):

```.net
> dotnet add package LightweightIocContainer.Validator --version 4.0.0-beta4
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

### Demo Project

There is a [demo project][demoProjectLink] available where you can check out how different functions of the Lightweight IOC Container can be used.

[demoProjectLink]: https://github.com/SimonG96/LightweightIocContainer_Example
