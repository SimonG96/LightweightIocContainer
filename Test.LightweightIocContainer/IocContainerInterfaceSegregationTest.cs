// Author: Simon Gockner
// Created: 2019-12-07
// Copyright(c) 2019 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using NUnit.Framework;

namespace Test.LightweightIocContainer;

[TestFixture]
public class IocContainerInterfaceSegregationTest
{
    private interface IBar
    {
            
    }

    private interface IFoo
    {
        void ThrowFoo();
    }
    private interface IAnotherBar
    {
            
    }

    private interface IAnotherFoo
    {
            
    }

    private interface IAnotherOne
    {
            
    }

    [UsedImplicitly]
    private class Foo : IFoo, IBar, IAnotherFoo, IAnotherBar, IAnotherOne
    {
        public void ThrowFoo() => throw new Exception("Foo");
    }


    private IocContainer _container;

    [SetUp]
    public void SetUp() => _container = new IocContainer();

    [TearDown]
    public void TearDown() => _container.Dispose();

    [Test]
    public void TestRegistrationOnCreate2()
    {
        _container.Register(r => r.Add<IBar, IFoo, Foo>().OnCreate(foo => foo.ThrowFoo()));

        Exception fooException = Assert.Throws<Exception>(() => _container.Resolve<IFoo>());
        Assert.That(fooException?.Message, Is.EqualTo("Foo"));
            
        Exception barException = Assert.Throws<Exception>(() => _container.Resolve<IBar>());
        Assert.That(barException?.Message, Is.EqualTo("Foo"));
    }

    [Test]
    public void TestRegistrationOnCreate3()
    {
        _container.Register(r => r.Add<IBar, IFoo, IAnotherFoo, Foo>().OnCreate(foo => foo.ThrowFoo()));

        Exception fooException = Assert.Throws<Exception>(() => _container.Resolve<IFoo>());
        Assert.That(fooException?.Message, Is.EqualTo("Foo"));

        Exception barException = Assert.Throws<Exception>(() => _container.Resolve<IBar>());
        Assert.That(barException?.Message, Is.EqualTo("Foo"));

        Exception anotherFooException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherFoo>());
        Assert.That(anotherFooException?.Message, Is.EqualTo("Foo"));
    }

    [Test]
    public void TestRegistrationOnCreate4()
    {
        _container.Register(r => r.Add<IBar, IFoo, IAnotherFoo, IAnotherBar, Foo>().OnCreate(foo => foo.ThrowFoo()));

        Exception fooException = Assert.Throws<Exception>(() => _container.Resolve<IFoo>());
        Assert.That(fooException?.Message, Is.EqualTo("Foo"));

        Exception barException = Assert.Throws<Exception>(() => _container.Resolve<IBar>());
        Assert.That(barException?.Message, Is.EqualTo("Foo"));

        Exception anotherFooException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherFoo>());
        Assert.That(anotherFooException?.Message, Is.EqualTo("Foo"));

        Exception anotherBarException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherBar>());
        Assert.That(anotherBarException?.Message, Is.EqualTo("Foo"));
    }

    [Test]
    public void TestRegistrationOnCreate5()
    {
        _container.Register(r => r.Add<IBar, IFoo, IAnotherFoo, IAnotherBar, IAnotherOne, Foo>().OnCreate(foo => foo.ThrowFoo()));

        Exception fooException = Assert.Throws<Exception>(() => _container.Resolve<IFoo>());
        Assert.That(fooException?.Message, Is.EqualTo("Foo"));

        Exception barException = Assert.Throws<Exception>(() => _container.Resolve<IBar>());
        Assert.That(barException?.Message, Is.EqualTo("Foo"));

        Exception anotherFooException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherFoo>());
        Assert.That(anotherFooException?.Message, Is.EqualTo("Foo"));

        Exception anotherBarException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherBar>());
        Assert.That(anotherBarException?.Message, Is.EqualTo("Foo"));

        Exception anotherOneException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherOne>());
        Assert.That(anotherOneException?.Message, Is.EqualTo("Foo"));
    }

    [Test]
    public void TestResolveTransient()
    {
        _container.Register(r => r.Add<IBar, IFoo, IAnotherFoo, IAnotherBar, IAnotherOne, Foo>());
        IFoo foo = _container.Resolve<IFoo>();
        IBar bar = _container.Resolve<IBar>();
        IAnotherFoo anotherFoo = _container.Resolve<IAnotherFoo>();
        IAnotherBar anotherBar = _container.Resolve<IAnotherBar>();
        IAnotherOne anotherOne = _container.Resolve<IAnotherOne>();

        Assert.That(foo, Is.InstanceOf<Foo>());
        Assert.That(bar, Is.InstanceOf<Foo>());
        Assert.That(anotherFoo, Is.InstanceOf<Foo>());
        Assert.That(anotherBar, Is.InstanceOf<Foo>());
        Assert.That(anotherOne, Is.InstanceOf<Foo>());
    }

    [Test]
    public void TestResolveSingleton()
    {
        _container.Register(r => r.Add<IBar, IFoo, IAnotherFoo, IAnotherBar, IAnotherOne, Foo>(Lifestyle.Singleton));
        IFoo foo = _container.Resolve<IFoo>();
        IBar bar = _container.Resolve<IBar>();
        IAnotherFoo anotherFoo = _container.Resolve<IAnotherFoo>();
        IAnotherBar anotherBar = _container.Resolve<IAnotherBar>();
        IAnotherOne anotherOne = _container.Resolve<IAnotherOne>();

        Assert.That(foo, Is.InstanceOf<Foo>());
        Assert.That(bar, Is.InstanceOf<Foo>());
        Assert.That(anotherFoo, Is.InstanceOf<Foo>());
        Assert.That(anotherBar, Is.InstanceOf<Foo>());
        Assert.That(anotherOne, Is.InstanceOf<Foo>());
        
        Assert.That(bar, Is.EqualTo(foo));
        Assert.That(anotherFoo, Is.EqualTo(foo));
        Assert.That(anotherBar, Is.EqualTo(foo));
        Assert.That(anotherOne, Is.EqualTo(foo));

        Assert.That(bar, Is.SameAs(foo));
        Assert.That(anotherFoo, Is.SameAs(foo));
        Assert.That(anotherBar, Is.SameAs(foo));
        Assert.That(anotherOne, Is.SameAs(foo));
    }
}