// Author: Gockner, Simon
// Created: 2019-11-22
// Copyright(c) 2019 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Registrations;
using NSubstitute;
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

    [UsedImplicitly]
    public interface IBar
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

        IocContainer iocContainerMock = Substitute.For<IocContainer>();
        iocContainerMock.Resolve<IBar>().Returns(bar);

        RegistrationFactory registrationFactory = new(iocContainerMock);
        ISingleTypeRegistration<IFoo> registration = registrationFactory.Register<IFoo>(Lifestyle.Transient).WithFactoryMethod(c => new Foo(c.Resolve<IBar>()));

        IFoo foo = registration.FactoryMethod!(iocContainerMock);
        Assert.That(foo.Bar, Is.EqualTo(bar));
    }

    [Test]
    public void TestSingleTypeRegistrationResolveSingleton()
    {
        IocContainer container = new();

        IBar bar = new Bar();
        container.Register(r => r.Add<IFoo>(Lifestyle.Singleton).WithFactoryMethod(_ => new Foo(bar)));

        IFoo foo = container.Resolve<IFoo>();

        Assert.That(foo, Is.InstanceOf<Foo>());
        Assert.That(foo.Bar, Is.EqualTo(bar));
    }
}