// Author: Gockner, Simon
// Created: 2021-12-09
// Copyright(c) 2021 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using NUnit.Framework;

namespace Test.LightweightIocContainer;

[TestFixture]
public class MultiLayerResolveTest
{
    public interface IA
    {
        
    }
    
    public interface IB
    {
        
    }
    
    [UsedImplicitly]
    public interface IAFactory
    {
        IA Create();
    }
    
    [UsedImplicitly]
    public interface IBFactory
    {
        IB Create(C c);
    }

    [UsedImplicitly]
    private class A : IA
    {
        [UsedImplicitly]
        private readonly IB _b;
        
        public A(IBFactory bFactory) => _b = bFactory.Create(new C("from A"));
    }

    [UsedImplicitly]
    private class B : IB
    {
        public B(C c)
        {
            
        }
    }
    
    [UsedImplicitly]
    public class C
    {
        public C(string test)
        {
            
        }
    }

    [Test]
    public void TestResolveFactoryAsCtorParameter()
    {
        IocContainer container = new();
        container.Register(r => r.Add<IA, A>().WithFactory<IAFactory>());
        container.Register(r => r.Add<IB, B>().WithFactory<IBFactory>());

        IA a = container.Resolve<IA>();
        Assert.IsInstanceOf<A>(a);
    }

    [Test]
    public void TestResolveSingleTypeRegistrationAsCtorParameter()
    {
        IocContainer container = new();
        container.Register(r => r.Add<IA, A>());
        container.Register(r => r.Add<IB, B>().WithFactory<IBFactory>());
        container.Register(r => r.Add<C>().WithFactoryMethod(_ => new C("test")));

        IB b = container.Resolve<IB>();
        Assert.IsInstanceOf<B>(b);
    }
}