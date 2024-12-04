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
}