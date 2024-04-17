// Author: Gockner, Simon
// Created: 2020-11-19
// Copyright(c) 2020 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using NUnit.Framework;

namespace Test.LightweightIocContainer;

[TestFixture]
public class MultipleMultitonRegistrationTest
{
    private IocContainer _iocContainer;
        
    [UsedImplicitly]
    public interface ITest : IProvider
    {
            
    }
        
    public interface IProvider
    {
        int Number { get; }
        void DoSomething(int number);
    }
        
    [UsedImplicitly]
    private class Test : ITest
    {
        public Test(MultitonScope scope)
        {
            
        }
        
        public int Number { get; private set; }

        public void DoSomething(int number) => Number = number;
    }

    private class MultitonScope
    {
            
    }
        
        
    [SetUp]
    public void SetUp() => _iocContainer = new IocContainer();

    [TearDown]
    public void TearDown() => _iocContainer.Dispose();

    [Test]
    public void TestRegisterAndResolveMultipleMultitonRegistration()
    {
        _iocContainer.Register(r => r.AddMultiton<IProvider, ITest, Test, MultitonScope>());

        MultitonScope scope = new();
            
        ITest test = _iocContainer.Resolve<ITest>(scope);
        Assert.That(test, Is.Not.Null);

        IProvider provider = _iocContainer.Resolve<IProvider>(scope);
        Assert.That(provider, Is.Not.Null);
        Assert.That(provider, Is.SameAs(test));
    }

    [Test]
    public void TestRegisterAndResolveMultipleMultitonRegistrationWithDifferentScope()
    {
        _iocContainer.Register(r => r.AddMultiton<IProvider, ITest, Test, MultitonScope>());

        MultitonScope scope = new();
        MultitonScope differentScope = new();
            
        ITest test = _iocContainer.Resolve<ITest>(scope);
        Assert.That(test, Is.Not.Null);

        IProvider provider = _iocContainer.Resolve<IProvider>(differentScope);
        Assert.That(provider, Is.Not.Null);
        
        Assert.That(provider, Is.Not.SameAs(test));
        Assert.That(provider, Is.Not.EqualTo(test));
    }

    [Test]
    public void TestMultipleMultitonRegistrationOnCreate()
    {
        _iocContainer.Register(r => r.AddMultiton<IProvider, ITest, Test, MultitonScope>().OnCreate(t => t.DoSomething(1)));
            
        MultitonScope scope = new();
            
        ITest test = _iocContainer.Resolve<ITest>(scope);
        Assert.That(test, Is.Not.Null);
        Assert.That(test.Number, Is.EqualTo(1));

        IProvider provider = _iocContainer.Resolve<IProvider>(scope);
        Assert.That(provider, Is.Not.Null);
        Assert.That(provider.Number, Is.EqualTo(1));
            
        Assert.That(provider, Is.SameAs(test));
    }
}