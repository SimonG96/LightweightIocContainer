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
public class OpenGenericRegistrationTest
{
    private IocContainer _iocContainer;

    [SetUp]
    public void SetUp() => _iocContainer = new IocContainer();

    [TearDown]
    public void TearDown() => _iocContainer.Dispose();
    
    [Test]
    public void TestRegisterFactoryOfOpenGenericType()
    {
        _iocContainer.Register(r => r.AddOpenGenerics(typeof(IGenericTest<>), typeof(GenericTest<>)).WithGeneratedFactory<IGenericTestFactory>());
        IGenericTestFactory testFactory = _iocContainer.Resolve<IGenericTestFactory>();
        IGenericTest<Constraint> test = testFactory.Create<Constraint>();
        Assert.That(test, Is.InstanceOf<GenericTest<Constraint>>());
    }

    [Test]
    public void TestRegisterFactoryOfOpenGenericTypeWithCtorParameter()
    {
        _iocContainer.Register(r => r.AddOpenGenerics(typeof(IGenericTest<>), typeof(CtorGenericTest<>)).WithGeneratedFactory<ICtorGenericTestFactory>());
        ICtorGenericTestFactory testFactory = _iocContainer.Resolve<ICtorGenericTestFactory>();
        IGenericTest<Constraint> test = testFactory.Create(new Constraint());
        Assert.That(test, Is.InstanceOf<CtorGenericTest<Constraint>>());
    }
}