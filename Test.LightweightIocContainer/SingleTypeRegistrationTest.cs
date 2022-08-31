﻿// Author: Gockner, Simon
// Created: 2019-11-22
// Copyright(c) 2019 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Registrations;
using Moq;
using NUnit.Framework;

namespace Test.LightweightIocContainer;

[TestFixture]
// ReSharper disable MemberHidesStaticFromOuterClass
public class SingleTypeRegistrationTest
{
    private interface IFoo
    {
        IBar Bar { get; }
    }

    private interface IBar
    {
            
    }

    [UsedImplicitly]
    private class Foo : IFoo
    {
        public Foo(IBar bar) => Bar = bar;

        public IBar Bar { get; }
    }

    [UsedImplicitly]
    private class Bar : IBar
    {
            
    }

    [Test]
    public void TestSingleTypeRegistrationWithFactoryMethod()
    {
        IBar bar = new Bar();

        Mock<IocContainer> iocContainerMock = new();
        iocContainerMock.Setup(c => c.Resolve<IBar>()).Returns(bar);

        RegistrationFactory registrationFactory = new(iocContainerMock.Object);
        ISingleTypeRegistration<IFoo> registration = registrationFactory.Register<IFoo>(Lifestyle.Transient).WithFactoryMethod(c => new Foo(c.Resolve<IBar>()));

        IFoo foo = registration.FactoryMethod!(iocContainerMock.Object);
        Assert.AreEqual(bar, foo.Bar);
    }

    [Test]
    public void TestSingleTypeRegistrationResolveSingleton()
    {
        IocContainer container = new();

        IBar bar = new Bar();
        container.Register(r => r.Add<IFoo>(Lifestyle.Singleton).WithFactoryMethod(_ => new Foo(bar)));

        IFoo foo = container.Resolve<IFoo>();

        Assert.IsInstanceOf<Foo>(foo);
        Assert.AreEqual(bar, foo.Bar);
    }
}