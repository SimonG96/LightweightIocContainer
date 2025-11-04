// Author: Gockner, Simon
// Created: 2021-12-16
// Copyright(c) 2021 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Exceptions;
using NUnit.Framework;

namespace Test.LightweightIocContainer;

[TestFixture]
public class DisposeStrategyTest
{
    [UsedImplicitly]
    public interface ITest : IDisposable;
    private class Test : ITest
    {
        public void Dispose() => throw new Exception();
    }
    
    private class TestMultiton : Test
    {
        public TestMultiton(int scope)
        {
            
        }
    }
    
    private class TestNotDisposable;

    [Test]
    public void TestValidContainerDisposeStrategySingleton()
    {
        IocContainer iocContainer = new();
        iocContainer.Register(r => r.Add<ITest, Test>(Lifestyle.Singleton).WithDisposeStrategy(DisposeStrategy.Container));

        iocContainer.Resolve<ITest>();
        
        Assert.Throws<Exception>(() => iocContainer.Dispose());
    }
    
    [Test]
    public void TestValidContainerDisposeStrategyMultiton()
    {
        IocContainer iocContainer = new();
        iocContainer.Register(r => r.AddMultiton<ITest, TestMultiton, int>().WithDisposeStrategy(DisposeStrategy.Container));

        iocContainer.Resolve<ITest>(1);
        
        Assert.Throws<Exception>(() => iocContainer.Dispose());
    }
    
    [Test]
    public void TestValidAppDisposeStrategySingleton()
    {
        IocContainer iocContainer = new();
        iocContainer.Register(r => r.Add<ITest, Test>(Lifestyle.Singleton).WithDisposeStrategy(DisposeStrategy.Application));

        iocContainer.Resolve<ITest>();
        
        Assert.DoesNotThrow(() => iocContainer.Dispose());
    }
    
    [Test]
    public void TestValidAppDisposeStrategyMultiton()
    {
        IocContainer iocContainer = new();
        iocContainer.Register(r => r.AddMultiton<ITest, TestMultiton, int>().WithDisposeStrategy(DisposeStrategy.Application));

        iocContainer.Resolve<ITest>(1);
        
        Assert.DoesNotThrow(() => iocContainer.Dispose());
    }
    
    [Test]
    public void TestInvalidDisposeStrategySingleton()
    {
        IocContainer iocContainer = new();
        Assert.Throws<InvalidDisposeStrategyException>(() => iocContainer.Register(r => r.Add<ITest, Test>(Lifestyle.Singleton)));
    }
    
    [Test]
    public void TestInvalidDisposeStrategyMultiton()
    {
        IocContainer iocContainer = new();
        Assert.Throws<InvalidDisposeStrategyException>(() => iocContainer.Register(r => r.AddMultiton<ITest, Test, int>()));
    }
    
    [Test]
    public void TestInvalidDisposeStrategyTransient()
    {
        IocContainer iocContainer = new();
        Assert.Throws<InvalidDisposeStrategyException>(() => iocContainer.Register(r => r.Add<ITest, Test>().WithDisposeStrategy(DisposeStrategy.Container)));
    }

    [Test]
    public void TestNoDisposeStrategyTypeNotDisposableSingleton()
    {
        IocContainer iocContainer = new();
        iocContainer.Register(r => r.Add<TestNotDisposable>(Lifestyle.Singleton));

        iocContainer.Resolve<TestNotDisposable>();
        
        Assert.DoesNotThrow(() => iocContainer.Dispose());
    }
    
    [Test]
    public void TestInvalidDisposeStrategyTypeNotDisposableSingleton()
    {
        IocContainer iocContainer = new();
        Assert.Throws<InvalidDisposeStrategyException>(() => iocContainer.Register(r => r.Add<TestNotDisposable>(Lifestyle.Singleton).WithDisposeStrategy(DisposeStrategy.Container)));
    }
}