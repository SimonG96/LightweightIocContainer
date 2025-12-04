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
public class MultiLayerResolveTest
{
    [Test]
    public void TestResolveFactoryAsCtorParameter()
    {
        IocContainer container = new();
        container.Register(r => r.Add<IA, A>().WithGeneratedFactory<IAFactory>());
        container.Register(r => r.Add<IB, B>().WithGeneratedFactory<IBFactory>());

        IAFactory aFactory = container.Resolve<IAFactory>();
        IA a = aFactory.Create();
        Assert.That(a, Is.InstanceOf<A>());
    }

    [Test]
    public void TestResolveSingleTypeRegistrationAsCtorParameter()
    {
        IocContainer container = new();
        container.Register(r => r.Add<IA, A>());
        container.Register(r => r.Add<IB, B>().WithGeneratedFactory<IBFactory>());
        container.Register(r => r.Add<C>().WithFactoryMethod(_ => new C("test")));

        IBFactory bFactory = container.Resolve<IBFactory>();
        IB b = bFactory.Create();
        Assert.That(b, Is.InstanceOf<B>());
    }
}