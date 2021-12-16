// Author: Gockner, Simon
// Created: 2019-06-12
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Installers;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;
using Moq;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    public class AssemblyInstallerTest
    {
        [UsedImplicitly]
        public class TestInstaller : IIocInstaller
        {
            public void Install(IRegistrationCollector registration) => registration.Add<Mock<IRegistration>>();
        }

        [UsedImplicitly]
        public class AssemblyWrapper : Assembly
        {

        }

        
        [Test]
        public void TestInstall()
        {
            List<Type> types = new()
            {
                typeof(object),
                typeof(TestInstaller)
            };

            Mock<AssemblyWrapper> assemblyMock = new();
            assemblyMock.Setup(a => a.GetTypes()).Returns(types.ToArray);

            Mock<IRegistrationCollector> registrationCollectorMock = new();

            AssemblyInstaller assemblyInstaller = new(assemblyMock.Object);
            assemblyInstaller.Install(registrationCollectorMock.Object);

            registrationCollectorMock.Verify(r => r.Add<It.IsSubtype<Mock<IRegistration>>>(It.IsAny<Lifestyle>()), Times.Once);
        }

        [Test]
        public void TestFromAssemblyThis()
        {
            IIocContainer iocContainer = new IocContainer();
            iocContainer.Install(FromAssembly.This());
        }

        [Test]
        public void TestFromAssemblyInstance()
        {
            List<Type> types = new()
            {
                typeof(object),
                typeof(TestInstaller)
            };

            Mock<AssemblyWrapper> assemblyMock = new();
            assemblyMock.Setup(a => a.GetTypes()).Returns(types.ToArray);

            IIocContainer iocContainer = new IocContainer();
            iocContainer.Install(FromAssembly.Instance(assemblyMock.Object));
        }
    }
}