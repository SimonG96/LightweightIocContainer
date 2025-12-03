// Author: Simon.Gockner
// Created: 2025-12-03
// Copyright(c) 2025 SimonG. All Rights Reserved.

using LightweightIocContainer;
using LightweightIocContainer.FactoryGenerator;
using Test.LightweightIocContainer.FactoryGenerator.TestClasses;
using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Factories;
using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

namespace Test.LightweightIocContainer.FactoryGenerator;

[TestFixture]
public class AsyncFactoryTest
{
    [Test]
    public async Task TestAsyncFactoryResolve()
    {
        IocContainer container = new();
        container.Register(r => r.Add<IAsyncTest, AsyncTest>().WithGeneratedFactory<IAsyncTestFactory>());

        IAsyncTestFactory testFactory = container.Resolve<IAsyncTestFactory>();
        IAsyncTest test = await testFactory.Create();
        
        Assert.That(test, Is.InstanceOf<AsyncTest>());
    }
    
    [Test]
    public async Task TestAsyncFactoryResolveOnCreateCalled()
    {
        IocContainer container = new();
        container.Register(r => r.Add<IAsyncTest, AsyncTest>().OnCreateAsync(t => t.Initialize()).WithGeneratedFactory<IAsyncTestFactory>());

        IAsyncTestFactory testFactory = container.Resolve<IAsyncTestFactory>();
        IAsyncTest test = await testFactory.Create();
        
        Assert.That(test, Is.InstanceOf<AsyncTest>());
        Assert.That(test.IsInitialized, Is.True);
    }

    [Test]
    public async Task TestAsyncMultitonFactoryResolveOnCreateCalledCorrectly()
    {
        IocContainer container = new();
        container.Register(r => r.AddMultiton<IAsyncTest, AsyncMultitonTest, int>().OnCreateAsync(t => t.Initialize()).WithGeneratedFactory<IAsyncMultitonTestFactory>());
        
        IAsyncMultitonTestFactory testFactory = container.Resolve<IAsyncMultitonTestFactory>();
        IAsyncTest test1 = await testFactory.Create(1);
        IAsyncTest test2 = await testFactory.Create(2);
        IAsyncTest anotherTest1 = await testFactory.Create(1);
        
        Assert.That(test1, Is.InstanceOf<AsyncTest>());
        Assert.That(test1.IsInitialized, Is.True);
        
        Assert.That(test2, Is.InstanceOf<AsyncTest>());
        Assert.That(test2.IsInitialized, Is.True);

        Assert.That(test1, Is.SameAs(anotherTest1));
    }
}