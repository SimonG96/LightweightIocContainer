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
        private class TestInstaller : IIocInstaller
        {
            public void Install(IIocContainer container) => container.Register<Mock<IRegistration>>();
        }

        [UsedImplicitly]
        public class AssemblyWrapper : Assembly
        {

        }

        
        [Test]
        public void TestInstall()
        {
            List<Type> types = new List<Type>
            {
                typeof(object),
                typeof(TestInstaller)
            };

            Mock<AssemblyWrapper> assemblyMock = new Mock<AssemblyWrapper>();
            assemblyMock.Setup(a => a.GetTypes()).Returns(types.ToArray);

            Mock<IIocContainer> iocContainerMock = new Mock<IIocContainer>();

            AssemblyInstaller assemblyInstaller = new AssemblyInstaller(assemblyMock.Object);
            assemblyInstaller.Install(iocContainerMock.Object);

            iocContainerMock.Verify(ioc => ioc.Register<It.IsSubtype<Mock<IRegistration>>>(It.IsAny<Lifestyle>()), Times.Once);
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
            List<Type> types = new List<Type>
            {
                typeof(object),
                typeof(TestInstaller)
            };

            Mock<AssemblyWrapper> assemblyMock = new Mock<AssemblyWrapper>();
            assemblyMock.Setup(a => a.GetTypes()).Returns(types.ToArray);

            IIocContainer iocContainer = new IocContainer();
            iocContainer.Install(FromAssembly.Instance(assemblyMock.Object));
        }
    }
}