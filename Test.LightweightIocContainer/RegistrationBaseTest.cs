// Author: Gockner, Simon
// Created: 2019-06-06
// Copyright(c) 2019 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Registrations;
using LightweightIocContainer.ResolvePlaceholders;
using NSubstitute;
using NUnit.Framework;

namespace Test.LightweightIocContainer;

[TestFixture]
public class RegistrationBaseTest
{
    private interface ITest;

    private interface IFoo;

    private interface IBar;

    private class Test : ITest;

    [UsedImplicitly]
    private class Foo : IFoo
    {
        public Foo(IBar bar, ITest test)
        {

        }
    }

    private class Bar : IBar;

    [Test]
    public void TestWithParameters()
    {
        RegistrationFactory registrationFactory = new(Substitute.For<IocContainer>());

        IBar bar = new Bar();
        ITest test = new Test();

        RegistrationBase testRegistration = (RegistrationBase) registrationFactory.Register<IFoo, Foo>(Lifestyle.Transient).WithParameters(bar, test);

        Assert.That(testRegistration.Parameters![0], Is.EqualTo(bar));
        Assert.That(testRegistration.Parameters[1], Is.EqualTo(test));
    }

    [Test]
    public void TestWithParametersDifferentOrder()
    {
        RegistrationFactory registrationFactory = new(Substitute.For<IocContainer>());

        IBar bar = new Bar();
        ITest test = new Test();

        RegistrationBase testRegistration = (RegistrationBase) registrationFactory.Register<IFoo, Foo>(Lifestyle.Transient).WithParameters((0, bar), (3, test), (2, "SomeString"));

        Assert.That(testRegistration.Parameters![0], Is.EqualTo(bar));
        Assert.That(testRegistration.Parameters[1], Is.InstanceOf<InternalResolvePlaceholder>());
        Assert.That(testRegistration.Parameters[2], Is.EqualTo("SomeString"));
        Assert.That(testRegistration.Parameters[3], Is.EqualTo(test));
    }

    [Test]
    public void TestWithParametersCalledTwice()
    {
        RegistrationFactory registrationFactory = new(Substitute.For<IocContainer>());
        Assert.Throws<InvalidRegistrationException>(() => registrationFactory.Register<IFoo, Foo>(Lifestyle.Transient).WithParameters(new Bar()).WithParameters(new Test()));
        Assert.Throws<InvalidRegistrationException>(() => registrationFactory.Register<IFoo, Foo>(Lifestyle.Transient).WithParameters((0, new Bar())).WithParameters((1, new Test())));
    }

    [Test]
    public void TestWithParametersNoParametersGiven()
    {
        RegistrationFactory registrationFactory = new(Substitute.For<IocContainer>());
        Assert.Throws<InvalidRegistrationException>(() => registrationFactory.Register<IFoo, Foo>(Lifestyle.Transient).WithParameters((object[])null));
        Assert.Throws<InvalidRegistrationException>(() => registrationFactory.Register<IFoo, Foo>(Lifestyle.Transient).WithParameters(((int index, object parameter)[])null));
    }
}