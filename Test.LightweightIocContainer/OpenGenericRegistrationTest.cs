// Author: Simon Gockner
// Created: 2020-09-18
// Copyright(c) 2020 SimonG. All Rights Reserved.

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Exceptions;
using NUnit.Framework;

namespace Test.LightweightIocContainer;

[TestFixture]
public class OpenGenericRegistrationTest
{
    private IocContainer _iocContainer;

    [UsedImplicitly]
    public interface IConstraint;
    
    [UsedImplicitly]
    public class Constraint : IConstraint;

    [UsedImplicitly]
    public class AnotherConstraint : IConstraint;
        
    [UsedImplicitly]
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public interface ITest<T> where T : IConstraint, new();
        
    [UsedImplicitly]
    public class Test<T> : ITest<T> where T : IConstraint, new();
    
    [UsedImplicitly]
    public class CtorTest<T> : ITest<T> where T : IConstraint, new()
    {
        public CtorTest(T item)
        {
            
        }
    }
    
    [UsedImplicitly]
    public interface ITestFactory
    {
        ITest<T> Create<T>() where T : IConstraint, new();
    }
    
    [UsedImplicitly]
    public interface ICtorTestFactory
    {
        ITest<T> Create<T>(T item) where T : IConstraint, new();
    }

    [UsedImplicitly]
    public interface IA
    {
        ITest<Constraint> Test { get; }
    }
    
    [UsedImplicitly]
    public class A(ITest<Constraint> test) : IA
    {
        public ITest<Constraint> Test { get; } = test;
    }

    [UsedImplicitly]
    public interface IB
    {
        ITest<AnotherConstraint> Test { get; }
    }
    
    [UsedImplicitly]
    public class B(ITest<AnotherConstraint> test) : IB
    {
        public ITest<AnotherConstraint> Test { get; } = test;
    }
    
    [UsedImplicitly]
    public interface IGenericClass<T> where T : IConstraint, new()
    {
        ITest<T> Test { get; }
    }
    
    [UsedImplicitly]
    public class GenericClass<T>(ITest<T> test) : IGenericClass<T> where T : IConstraint, new()
    {
        public ITest<T> Test { get; } = test;
    }

    [SetUp]
    public void SetUp() => _iocContainer = new IocContainer();

    [TearDown]
    public void TearDown() => _iocContainer.Dispose();

    [Test]
    public void TestRegisterOpenGenericType()
    {
        _iocContainer.Register(r => r.AddOpenGenerics(typeof(ITest<>), typeof(Test<>)));

        ITest<Constraint> test = _iocContainer.Resolve<ITest<Constraint>>();
        Assert.That(test, Is.Not.Null);
    }

    [Test]
    public void TestRegisterOpenGenericTypeAsSingleton()
    {
        _iocContainer.Register(r => r.AddOpenGenerics(typeof(ITest<>), typeof(Test<>), Lifestyle.Singleton));

        ITest<Constraint> test = _iocContainer.Resolve<ITest<Constraint>>();
        Assert.That(test, Is.Not.Null);

        ITest<Constraint> secondTest = _iocContainer.Resolve<ITest<Constraint>>();
        Assert.That(secondTest, Is.Not.Null);
        
        Assert.That(secondTest, Is.EqualTo(test));
        Assert.That(secondTest, Is.SameAs(test));
    }
        
    [Test]
    public void TestRegisterOpenGenericTypeAsMultitonThrowsException() => 
        Assert.Throws<InvalidRegistrationException>(() => _iocContainer.Register(r => r.AddOpenGenerics(typeof(ITest<>), typeof(Test<>), Lifestyle.Multiton)));

    [Test]
    public void TestRegisterNonOpenGenericTypeWithOpenGenericsFunctionThrowsException() =>
        Assert.Throws<InvalidRegistrationException>(() => _iocContainer.Register(r => r.AddOpenGenerics(typeof(int), typeof(int))));

    [Test]
    public void TestRegisterFactoryOfOpenGenericType()
    {
        _iocContainer.Register(r => r.AddOpenGenerics(typeof(ITest<>), typeof(Test<>)).WithFactory<ITestFactory>());
        ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();
        ITest<Constraint> test = testFactory.Create<Constraint>();
        Assert.That(test, Is.InstanceOf<Test<Constraint>>());
    }

    [Test]
    public void TestRegisterFactoryOfOpenGenericTypeWithCtorParameter()
    {
        _iocContainer.Register(r => r.AddOpenGenerics(typeof(ITest<>), typeof(CtorTest<>)).WithFactory<ICtorTestFactory>());
        ICtorTestFactory testFactory = _iocContainer.Resolve<ICtorTestFactory>();
        ITest<Constraint> test = testFactory.Create(new Constraint());
        Assert.That(test, Is.InstanceOf<CtorTest<Constraint>>());
    }

    [Test]
    public void TestOpenGenericTypeAsParameter()
    {
        _iocContainer.Register(r => r.Add<IA, A>());
        _iocContainer.Register(r => r.AddOpenGenerics(typeof(ITest<>), typeof(Test<>), Lifestyle.Singleton));
        
        IA a = _iocContainer.Resolve<IA>();
        Assert.That(a, Is.TypeOf<A>());
    }
    
    [Test]
    public void TestOpenGenericTypeAsParameterDifferentGenericParametersResolveDifferentSingletons()
    {
        _iocContainer.Register(r => r.Add<IA, A>());
        _iocContainer.Register(r => r.Add<IB, B>());
        _iocContainer.Register(r => r.AddOpenGenerics(typeof(ITest<>), typeof(Test<>), Lifestyle.Singleton));
        
        IA a = _iocContainer.Resolve<IA>();
        Assert.That(a, Is.TypeOf<A>());
        
        IB b = _iocContainer.Resolve<IB>();
        Assert.That(b, Is.TypeOf<B>());
        
        Assert.That(b.Test, Is.Not.SameAs(a.Test));
    }

    [Test]
    public void TestOpenGenericTypeAsGenericParameter()
    {
        _iocContainer.Register(r => r.AddOpenGenerics(typeof(IGenericClass<>), typeof(GenericClass<>)));
        _iocContainer.Register(r => r.AddOpenGenerics(typeof(ITest<>), typeof(Test<>), Lifestyle.Singleton));
        
        IGenericClass<Constraint> genericClass = _iocContainer.Resolve<IGenericClass<Constraint>>();
        Assert.That(genericClass, Is.InstanceOf<GenericClass<Constraint>>());
    }
}