// Author: Simon.Gockner
// Created: 2025-12-03
// Copyright(c) 2025 SimonG. All Rights Reserved.

using LightweightIocContainer;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.FactoryGenerator;
using Test.LightweightIocContainer.FactoryGenerator.TestClasses;
using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Factories;
using Test.LightweightIocContainer.FactoryGenerator.TestClasses.Interfaces;

namespace Test.LightweightIocContainer.FactoryGenerator;

[TestFixture]
public class FluentFactoryRegistrationTest
{
    private IocContainer _iocContainer;

    [SetUp]
    public void SetUp() => _iocContainer = new IocContainer();

    [TearDown]
    public void TearDown() => _iocContainer.Dispose();
        
    [Test]
    public void TestFluentFactoryRegistration()
    {
        _iocContainer.Register(r => r.Add<IDefaultTest, DefaultTest>().WithGeneratedFactory<IDefaultTestFactory>());

        IDefaultTestFactory factory = _iocContainer.Resolve<IDefaultTestFactory>();
        IDefaultTest test = factory.Create();
        
        Assert.That(factory, Is.InstanceOf<IDefaultTestFactory>());
        Assert.That(test, Is.InstanceOf<IDefaultTest>());
    }
        
    [Test]
    public void TestFluentFactoryRegistrationResolveWithoutFactoryFails()
    {
        _iocContainer.Register(r => r.Add<IDefaultTest, DefaultTest>().WithGeneratedFactory<IDefaultTestFactory>());
        Assert.Throws<DirectResolveWithRegisteredFactoryNotAllowed>(()=>_iocContainer.Resolve<IDefaultTest>());
    }
        
    [Test]
    public void TestRegisterFactoryWithoutCreate() => Assert.Throws<InvalidFactoryRegistrationException>(() => _iocContainer.Register(r => r.Add<IDefaultTest, DefaultTest>().WithGeneratedFactory<ITestFactoryNoCreate>()));
        
    [Test]
    public void TestResolveFromFactory()
    {
        _iocContainer.Register(r => r.Add<IDefaultTest, DefaultTest>().WithGeneratedFactory<IDefaultTestFactory>());
        
        IDefaultTestFactory testFactory = _iocContainer.Resolve<IDefaultTestFactory>();
        IDefaultTest createdTest = testFactory.Create();
        
        Assert.That(createdTest, Is.InstanceOf<DefaultTest>());
    }
        
    [Test]
    public void TestResolveFromFactoryWithParams()
    {
        _iocContainer.Register(r => r.Add<IDefaultTest, TestConstructor>().WithGeneratedFactory<IDefaultTestFactory>());
        _iocContainer.Register(r => r.Add<DefaultTest, DefaultTest>()); //this registration is abnormal and should only be used in unit tests
        
        IDefaultTestFactory testFactory = _iocContainer.Resolve<IDefaultTestFactory>();
        IDefaultTest createdTest = testFactory.Create("Test");
        
        Assert.That(createdTest, Is.InstanceOf<TestConstructor>());
    }
        
    [Test]
    public void TestResolveFromFactoryWithDefaultParamCreate()
    {
        _iocContainer.Register(r => r.Add<IDefaultTest, TestConstructor>().WithGeneratedFactory<IDefaultTestFactory>());
        _iocContainer.Register(r => r.Add<DefaultTest, DefaultTest>()); //this registration is abnormal and should only be used in unit tests
        
        IDefaultTestFactory testFactory = _iocContainer.Resolve<IDefaultTestFactory>();
        IDefaultTest createdTest = testFactory.CreateTest();
        
        Assert.That(createdTest, Is.InstanceOf<TestConstructor>());
    }
        
    [Test]
    public void TestResolveFromFactoryWithDefaultParamCtor()
    {
        _iocContainer.Register(r => r.Add<IDefaultTest, TestConstructor>().WithGeneratedFactory<IDefaultTestFactory>());
        _iocContainer.Register(r => r.Add<DefaultTest, DefaultTest>()); //this registration is abnormal and should only be used in unit tests
        
        IDefaultTestFactory testFactory = _iocContainer.Resolve<IDefaultTestFactory>();
        IDefaultTest createdTest = testFactory.Create();
        
        Assert.That(createdTest, Is.InstanceOf<TestConstructor>());
    }
        
    [Test]
    public void TestResolveFromFactoryWithByte()
    {
        _iocContainer.Register(r => r.Add<IDefaultTest, TestByte>().WithGeneratedFactory<IDefaultTestFactory>());
        
        IDefaultTestFactory testFactory = _iocContainer.Resolve<IDefaultTestFactory>();
        IDefaultTest createdTest = testFactory.Create(1);
        
        Assert.That(createdTest, Is.InstanceOf<TestByte>());
    }

    [Test]
    public void TestPassingNullAsMiddleParameter()
    {
        _iocContainer.Register(r => r.Add<IDefaultTest, TestNull>().WithGeneratedFactory<ITestNullFactory>());

        ITestNullFactory testNullFactory = _iocContainer.Resolve<ITestNullFactory>();

        object obj = new();
        string content = "TestContent";
        string optional2 = "optionalParameter2";

        IDefaultTest createdTest = testNullFactory.Create(obj, content, null!, optional2);
        if (createdTest is not TestNull testNull)
        {
            Assert.Fail();
            return;
        }
        
        Assert.That(testNull.Obj, Is.SameAs(obj));
        Assert.That(testNull.Content, Is.EqualTo(content));
        Assert.That(testNull.Optional1, Is.Null);
        Assert.That(testNull.Optional2, Is.EqualTo(optional2));
    }
    
    [Test]
    public void TestPassingNullAsDefaultParameter()
    {
        _iocContainer.Register(r => r.Add<IDefaultTest, TestNull>().WithGeneratedFactory<ITestDefaultFactory>());

        ITestDefaultFactory testDefaultFactory = _iocContainer.Resolve<ITestDefaultFactory>();

        object obj = new();
        string content = "TestContent";
        string optional2 = "optionalParameter2";

        IDefaultTest createdTest = testDefaultFactory.Create(obj, content, null!, optional2);
        if (createdTest is not TestNull testNull)
        {
            Assert.Fail();
            return;
        }
        
        Assert.That(testNull.Obj, Is.SameAs(obj));
        Assert.That(testNull.Content, Is.EqualTo(content));
        Assert.That(testNull.Optional1, Is.Null);
        Assert.That(testNull.Optional2, Is.EqualTo(optional2));
        Assert.That(testNull.TestNullFactory, Is.Null);
    }
        
    [Test]
    public void TestResolveMultitonFromFactory()
    {
        _iocContainer.Register(r => r.AddMultiton<IDefaultTest, TestMultiton, MultitonScope>().WithGeneratedFactory<IMultitonTestFactory>());
        
        MultitonScope scope1 = new();
        MultitonScope scope2 = new();
        
        IMultitonTestFactory testFactory = _iocContainer.Resolve<IMultitonTestFactory>();
        
        IDefaultTest resolvedTest1 = testFactory.Create(scope1);
        IDefaultTest resolvedTest2 = testFactory.Create(scope1);
        IDefaultTest resolvedTest3 = testFactory.Create(scope2);
        
        Assert.That(resolvedTest1, Is.SameAs(resolvedTest2));
        Assert.That(resolvedTest1, Is.Not.SameAs(resolvedTest3));
        Assert.That(resolvedTest2, Is.Not.SameAs(resolvedTest3));
    }
        
    [Test]
    public void TestResolveMultitonFromFactoryClearInstances()
    {
        _iocContainer.Register(r => r.AddMultiton<IDefaultTest, TestMultiton, MultitonScope>().WithGeneratedFactory<IMultitonTestFactory>());
        
        MultitonScope scope1 = new();
        MultitonScope scope2 = new();
        
        IMultitonTestFactory testFactory = _iocContainer.Resolve<IMultitonTestFactory>();
        
        IDefaultTest resolvedTest1 = testFactory.Create(scope1);
        IDefaultTest resolvedTest2 = testFactory.Create(scope1);
        IDefaultTest resolvedTest3 = testFactory.Create(scope2);
        
        Assert.That(resolvedTest1, Is.SameAs(resolvedTest2));
        Assert.That(resolvedTest1, Is.Not.SameAs(resolvedTest3));
        Assert.That(resolvedTest2, Is.Not.SameAs(resolvedTest3));
        
        testFactory.ClearMultitonInstance<IDefaultTest>();
        
        IDefaultTest resolvedTest4 = testFactory.Create(scope1);
        IDefaultTest resolvedTest5 = testFactory.Create(scope2);
        
        Assert.That(resolvedTest1, Is.Not.SameAs(resolvedTest4));
        Assert.That(resolvedTest2, Is.Not.SameAs(resolvedTest4));
        Assert.That(resolvedTest3, Is.Not.SameAs(resolvedTest5));
    }

    [Test]
    public void InvalidMultitonFactoryRegistrationFactoryWithoutParameter() => 
        Assert.Throws<InvalidFactoryRegistrationException>(() => _iocContainer.Register(r => r.AddMultiton<IDefaultTest, DefaultTest, MultitonScope>().WithGeneratedFactory<IDefaultTestFactory>()));
        
    [Test]
    public void InvalidMultitonFactoryRegistrationFactoryWithoutScopeAsFirstParameter() => 
        Assert.Throws<InvalidFactoryRegistrationException>(() => _iocContainer.Register(r => r.AddMultiton<IDefaultTest, DefaultTest, MultitonScope>().WithGeneratedFactory<IInvalidMultitonTestFactory>()));

    [Test]
    public void TestInvalidCreateMethodReturnType() => 
        Assert.Throws<InvalidFactoryRegistrationException>(() => _iocContainer.Register(r => r.Add<IDefaultTest, DefaultTest>().WithGeneratedFactory<ITestFactoryWrongReturn>()));
}