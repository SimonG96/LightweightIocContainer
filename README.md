# Lightweight IOC Container

A lightweight IOC Container that is powerful enough to do all the things you need it to do.  
*This project and its documentation are currently under development.*

## Get started with the Lightweight IOC Container

### How to install <!--TODO: Add links to wiki-->

The easiest way to install the Lightweight IOC Container is by using [NuGet](https://www.nuget.org/packages/LightweightIocContainer/).  
You can either use the `PackageManager` in VisualStudio:

```PM
PM> Install-Package LightweightIocContainer -Version 1.0.0
```

or you can use the `.NET CLI`:

```.net
> dotnet add package LightweightIocContainer --version 1.0.0
```

### Example usage

  1. Instantiate container:
  
      ```c#
      IocContainer container = new IocContainer();
      ```

  2. Install `IIocInstaller`s for the container:

      ```c#
      container.Install(new Installer());
      ```

  3. Resolve one instance from the container:

      ```c#
      IFooFactory fooFactory = container.Resolve<IFooFactory>();
      ```

  4. Use this instance to create what your application needs:

      ```c#
      IFoo foo = fooFactory.Create();
      ```

  5. When your application is finished, don't forget to dispose your `IocContainer`:

      ```c#
      container.Dispose();
      ```

### Demo Project

There is a [demo project][demoProjectLink] available where you can check out how different functions of the Lightweight IOC Container can be used.  
This demo project is updated with new versions of the Lightweight IOC Container to show the usage of newly implemented functions.

[demoProjectLink]: https://github.com/SimonG96/LightweightIocContainer_Example
