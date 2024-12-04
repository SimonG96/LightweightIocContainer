// Author: Gockner, Simon
// Created: 2019-06-12
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System.Reflection;
using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Installers;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;
using NSubstitute;
using NUnit.Framework;

namespace Test.LightweightIocContainer;

[TestFixture]
public class AssemblyInstallerTest
{
    [UsedImplicitly]
    public class TestInstaller : IIocInstaller
    {
        public void Install(IRegistrationCollector registration) => registration.Add<IRegistration>();
    }

    [UsedImplicitly]
    public class AssemblyWrapper : Assembly;
        
    [Test]
    public void TestInstall()
    {
        List<Type> types =
        [
            typeof(object),
            typeof(TestInstaller)
        ];

        AssemblyWrapper assemblyMock = Substitute.For<AssemblyWrapper>();
        assemblyMock.GetTypes().Returns(types.ToArray());

        IRegistrationCollector registrationCollectorMock = Substitute.For<IRegistrationCollector>();

        AssemblyInstaller assemblyInstaller = new(assemblyMock);
        assemblyInstaller.Install(registrationCollectorMock);

        registrationCollectorMock.Received(1).Add<IRegistration>(Arg.Any<Lifestyle>());
    }

    [Test]
    public void TestFromAssemblyThis()
    {
        IocContainer iocContainer = new();
        iocContainer.Install(FromAssembly.This());
    }

    [Test]
    public void TestFromAssemblyInstance()
    {
        List<Type> types =
        [
            typeof(object),
            typeof(TestInstaller)
        ];

        AssemblyWrapper assemblyMock = Substitute.For<AssemblyWrapper>();
        assemblyMock.GetTypes().Returns(types.ToArray());

        IocContainer iocContainer = new();
        iocContainer.Install(FromAssembly.Instance(assemblyMock));
    }
}