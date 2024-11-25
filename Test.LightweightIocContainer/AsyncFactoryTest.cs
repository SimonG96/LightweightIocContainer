// Author: simon.gockner
// Created: 2024-11-25
// Copyright(c) 2024 SimonG. All Rights Reserved.

using LightweightIocContainer;
using NUnit.Framework;

namespace Test.LightweightIocContainer;

[TestFixture]
public class AsyncFactoryTest
{
    public interface ITest
    {
        bool IsInitialized { get; }
        Task Initialize();
    }
    
    public class Test : ITest
    {
        public bool IsInitialized { get; private set; }

        public virtual async Task Initialize()
        {
            await Task.Delay(200);
            IsInitialized = true;
        }
    }

    public class MultitonTest(int id) : Test
    {
        public int Id { get; } = id;
        public override async Task Initialize()
        {
            if (IsInitialized)
                throw new Exception();
            
            await base.Initialize();
        }
    }
    
    public interface ITestFactory
    {
        Task<ITest> Create();
    }
    
    public interface IMultitonTestFactory
    {
        Task<ITest> Create(int id);
    }
    
    [Test]
    public async Task TestAsyncFactoryResolve()
    {
        IocContainer container = new();
        container.Register(r => r.Add<ITest, Test>().WithFactory<ITestFactory>());

        ITestFactory testFactory = container.Resolve<ITestFactory>();
        ITest test = await testFactory.Create();
        
        Assert.That(test, Is.InstanceOf<Test>());
    }
    
    [Test]
    public async Task TestAsyncFactoryResolveOnCreateCalled()
    {
        IocContainer container = new();
        container.Register(r => r.Add<ITest, Test>().OnCreateAsync(t => t.Initialize()).WithFactory<ITestFactory>());

        ITestFactory testFactory = container.Resolve<ITestFactory>();
        ITest test = await testFactory.Create();
        
        Assert.That(test, Is.InstanceOf<Test>());
        Assert.That(test.IsInitialized, Is.True);
    }

    [Test]
    public async Task TestAsyncMultitonFactoryResolveOnCreateCalledCorrectly()
    {
        IocContainer container = new();
        container.Register(r => r.AddMultiton<ITest, MultitonTest, int>().OnCreateAsync(t => t.Initialize()).WithFactory<IMultitonTestFactory>());
        
        IMultitonTestFactory testFactory = container.Resolve<IMultitonTestFactory>();
        ITest test1 = await testFactory.Create(1);
        ITest test2 = await testFactory.Create(2);
        ITest anotherTest1 = await testFactory.Create(1);
        
        Assert.That(test1, Is.InstanceOf<Test>());
        Assert.That(test1.IsInitialized, Is.True);
        
        Assert.That(test2, Is.InstanceOf<Test>());
        Assert.That(test2.IsInitialized, Is.True);

        Assert.That(test1, Is.SameAs(anotherTest1));
    }
}