# Lightweight IOC Container

A lightweight IOC Container that is powerful enough to do all the things you need it to do.  

[![GitHub Actions](https://github.com/SimonG96/LightweightIocContainer/workflows/CI/badge.svg)](https://github.com/SimonG96/LightweightIocContainer/actions)

[![Nuget](https://img.shields.io/nuget/dt/LightweightIocContainer.svg?label=NuGet%20Downloads&logo=NuGet)](https://www.nuget.org/packages/LightweightIocContainer/)
[![Nuget](https://img.shields.io/nuget/v/LightweightIocContainer.svg?label=NuGet%20Version&logo=NuGet)](https://www.nuget.org/packages/LightweightIocContainer/)
[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/LightweightIocContainer.svg?label=NuGet%20Pre-Release&logo=NuGet)](https://www.nuget.org/packages/LightweightIocContainer/)

## Get started with the Lightweight IOC Container

### How to install

The easiest way to [install](https://github.com/SimonG96/LightweightIocContainer/wiki/Install-Lightweight-IOC-Container) the Lightweight IOC Container is by using [NuGet](https://www.nuget.org/packages/LightweightIocContainer/).  
You can either use the [`PackageManager`](https://github.com/SimonG96/LightweightIocContainer/wiki/Install-Lightweight-IOC-Container#packagemanager) in VisualStudio:

```PM
PM> Install-Package LightweightIocContainer -Version 2.2.0-beta
```

or you can use the [`.NET CLI`](https://github.com/SimonG96/LightweightIocContainer/wiki/Install-Lightweight-IOC-Container#net-cli):

```.net
> dotnet add package LightweightIocContainer --version 2.2.0-beta
```

### Example usage

  1. [Instantiate `IocContainer`](https://github.com/SimonG96/LightweightIocContainer/wiki/Simple-Usage-of-Lightweight-IOC-Container#instantiate-container):
  
      ```c#
      IocContainer container = new IocContainer();
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

### Demo Project

There is a [demo project][demoProjectLink] available where you can check out how different functions of the Lightweight IOC Container can be used.

[demoProjectLink]: https://github.com/SimonG96/LightweightIocContainer_Example
