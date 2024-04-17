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
        IB BProperty { get; }
    }
    
    public interface IB
    {
        C C { get; }
    }
    
    [UsedImplicitly]
    public interface IAFactory
    {
        IA Create();
    }
    
    [UsedImplicitly]
    public interface IBFactory
    {
        IB Create();
        IB Create(C c);
    }

    [UsedImplicitly]
    private class A : IA
    {
        public A(IBFactory bFactory) => BProperty = bFactory.Create(new C("from A"));
        public IB BProperty { get; }
    }
    
    private class OtherA : IA
    {
        public OtherA(IB bProperty, IB secondB)
        {
            BProperty = bProperty;
            SecondB = secondB;
        }

        public IB BProperty { get; }
        public IB SecondB { get; }
    }

    [UsedImplicitly]
    private class B : IB
    {
        public B(C c) => C = c;

        public C C { get; }
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

        IAFactory aFactory = container.Resolve<IAFactory>();
        IA a = aFactory.Create();
        Assert.That(a, Is.InstanceOf<A>());
    }

    [Test]
    public void TestResolveSingleTypeRegistrationAsCtorParameter()
    {
        IocContainer container = new();
        container.Register(r => r.Add<IA, A>());
        container.Register(r => r.Add<IB, B>().WithFactory<IBFactory>());
        container.Register(r => r.Add<C>().WithFactoryMethod(_ => new C("test")));

        IBFactory bFactory = container.Resolve<IBFactory>();
        IB b = bFactory.Create();
        Assert.That(b, Is.InstanceOf<B>());
    }

    [Test]
    public void TestResolveSingletonTwiceAsCtorParameterInSameCtor()
    {
        IocContainer container = new();
        container.Register(r => r.Add<IA, OtherA>());
        container.Register(r => r.Add<IB, B>());
        container.Register(r => r.Add<C>(Lifestyle.Singleton).WithParameters("test"));

        OtherA a = container.Resolve<OtherA>();
        Assert.That(a.SecondB.C, Is.EqualTo(a.BProperty.C));
    }
}