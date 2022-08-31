﻿// Author: Gockner, Simon
// Created: 2021-11-29
// Copyright(c) 2021 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Exceptions;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    public class FluentFactoryRegistrationTest
    {
        public interface ITest
        {
            
        }

        private class Test : ITest
        {
            
        }
        
        private class TestByte : ITest
        {
            [UsedImplicitly]
            private readonly byte _id;

            public TestByte(byte id) => _id = id;
        }
        
        [UsedImplicitly]
        private class TestConstructor : ITest
        {
            public TestConstructor(string name, Test test)
            {

            }

            public TestConstructor(Test test, string name = null)
            {

            }
        }
        
        private interface ITestFactoryNoCreate
        {
            
        }

        private interface ITestFactoryNonGenericClear
        {
            ITest Create();

            void ClearMultitonInstance();
        }
        
        [UsedImplicitly]
        public interface ITestFactory
        {
            ITest Create();
            ITest Create(string name);
            ITest CreateTest(string name = null);
            ITest Create(byte id);
        }
        
        private class TestFactory : ITestFactory
        {
            public ITest Create() => new Test();
            public ITest Create(string name) => throw new System.NotImplementedException();
            public ITest CreateTest(string name = null) => throw new System.NotImplementedException();
            public ITest Create(byte id) => throw new System.NotImplementedException();
        }
        
        [UsedImplicitly]
        public interface IMultitonTestFactory
        {
            ITest Create(MultitonScope scope);
            void ClearMultitonInstance<T>();
        }
        
        [UsedImplicitly]
        public interface IInvalidMultitonTestFactory
        {
            ITest Create(MultitonScope scope);
            ITest Create(int number);
        }
        
        [UsedImplicitly]
        public interface ITestFactoryWrongReturn
        {
            public MultitonScope Create();
        }
        
        public class MultitonScope
        {

        }
        
        private IocContainer _iocContainer;

        [SetUp]
        public void SetUp() => _iocContainer = new IocContainer();

        [TearDown]
        public void TearDown() => _iocContainer.Dispose();
        
        [Test]
        public void TestFluentFactoryRegistration()
        {
            _iocContainer.Register(r => r.Add<ITest, Test>().WithFactory<ITestFactory>());

            ITestFactory factory = _iocContainer.Resolve<ITestFactory>();
            ITest test = factory.Create();
            
            Assert.IsInstanceOf<ITestFactory>(factory);
            Assert.IsInstanceOf<ITest>(test);
        }
        
        [Test]
        public void TestFluentFactoryRegistration_CustomFactory()
        {
            _iocContainer.Register(r => r.Add<ITest, Test>().WithFactory<ITestFactory, TestFactory>());

            ITestFactory factory = _iocContainer.Resolve<ITestFactory>();
            ITest test = factory.Create();
            
            Assert.IsInstanceOf<ITestFactory>(factory);
            Assert.IsInstanceOf<ITest>(test);
        }
        
        [Test]
        public void TestRegisterFactoryWithoutCreate() => Assert.Throws<InvalidFactoryRegistrationException>(() => _iocContainer.Register(r => r.Add<ITest, Test>().WithFactory<ITestFactoryNoCreate>()));
        
        [Test]
        public void TestRegisterFactoryClearMultitonsNonGeneric() => Assert.Throws<IllegalAbstractMethodCreationException>(() => _iocContainer.Register(r => r.Add<ITest, Test>().WithFactory<ITestFactoryNonGenericClear>()));
        
        [Test]
        public void TestResolveFromFactory()
        {
            _iocContainer.Register(r => r.Add<ITest, Test>().WithFactory<ITestFactory>());
        
            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.Create();
        
            Assert.IsInstanceOf<Test>(createdTest);
        }
        
        [Test]
        public void TestResolveFromFactoryWithParams()
        {
            _iocContainer.Register(r => r.Add<ITest, TestConstructor>().WithFactory<ITestFactory>());
            _iocContainer.Register(r => r.Add<Test, Test>()); //this registration is abnormal and should only be used in unit tests
        
            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.Create("Test");
        
            Assert.IsInstanceOf<TestConstructor>(createdTest);
        }
        
        [Test]
        public void TestResolveFromFactoryWithDefaultParamCreate()
        {
            _iocContainer.Register(r => r.Add<ITest, TestConstructor>().WithFactory<ITestFactory>());
            _iocContainer.Register(r => r.Add<Test, Test>()); //this registration is abnormal and should only be used in unit tests
        
            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.CreateTest();
        
            Assert.IsInstanceOf<TestConstructor>(createdTest);
        }
        
        [Test]
        public void TestResolveFromFactoryWithDefaultParamCtor()
        {
            _iocContainer.Register(r => r.Add<ITest, TestConstructor>().WithFactory<ITestFactory>());
            _iocContainer.Register(r => r.Add<Test, Test>()); //this registration is abnormal and should only be used in unit tests
        
            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.Create();
        
            Assert.IsInstanceOf<TestConstructor>(createdTest);
        }
        
        [Test]
        public void TestResolveFromFactoryWithByte()
        {
            _iocContainer.Register(r => r.Add<ITest, TestByte>().WithFactory<ITestFactory>());
        
            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.Create(1);
        
            Assert.IsInstanceOf<TestByte>(createdTest);
        }
        
        [Test]
        public void TestResolveMultitonFromFactory()
        {
            _iocContainer.Register(r => r.AddMultiton<ITest, Test, MultitonScope>().WithFactory<IMultitonTestFactory>());
        
            MultitonScope scope1 = new();
            MultitonScope scope2 = new();
        
            IMultitonTestFactory testFactory = _iocContainer.Resolve<IMultitonTestFactory>();
        
            ITest resolvedTest1 = testFactory.Create(scope1);
            ITest resolvedTest2 = testFactory.Create(scope1);
            ITest resolvedTest3 = testFactory.Create(scope2);
        
            Assert.AreSame(resolvedTest1, resolvedTest2);
            Assert.AreNotSame(resolvedTest1, resolvedTest3);
            Assert.AreNotSame(resolvedTest2, resolvedTest3);
        }
        
        [Test]
        public void TestResolveMultitonFromFactoryClearInstances()
        {
            _iocContainer.Register(r => r.AddMultiton<ITest, Test, MultitonScope>().WithFactory<IMultitonTestFactory>());
        
            MultitonScope scope1 = new();
            MultitonScope scope2 = new();
        
            IMultitonTestFactory testFactory = _iocContainer.Resolve<IMultitonTestFactory>();
        
            ITest resolvedTest1 = testFactory.Create(scope1);
            ITest resolvedTest2 = testFactory.Create(scope1);
            ITest resolvedTest3 = testFactory.Create(scope2);
        
            Assert.AreSame(resolvedTest1, resolvedTest2);
            Assert.AreNotSame(resolvedTest1, resolvedTest3);
            Assert.AreNotSame(resolvedTest2, resolvedTest3);
        
            testFactory.ClearMultitonInstance<ITest>();
        
            ITest resolvedTest4 = testFactory.Create(scope1);
            ITest resolvedTest5 = testFactory.Create(scope2);
        
            Assert.AreNotSame(resolvedTest1, resolvedTest4);
            Assert.AreNotSame(resolvedTest2, resolvedTest4);
            Assert.AreNotSame(resolvedTest3, resolvedTest5);
        }

        [Test]
        public void InvalidMultitonFactoryRegistrationFactoryWithoutParameter() => 
            Assert.Throws<InvalidFactoryRegistrationException>(() => _iocContainer.Register(r => r.AddMultiton<ITest, Test, MultitonScope>().WithFactory<ITestFactory>()));
        
        [Test]
        public void InvalidMultitonFactoryRegistrationFactoryWithoutScopeAsFirstParameter() => 
            Assert.Throws<InvalidFactoryRegistrationException>(() => _iocContainer.Register(r => r.AddMultiton<ITest, Test, MultitonScope>().WithFactory<IInvalidMultitonTestFactory>()));

        [Test]
        public void TestInvalidCreateMethodReturnType() => 
            Assert.Throws<InvalidFactoryRegistrationException>(() => _iocContainer.Register(r => r.Add<ITest, Test>().WithFactory<ITestFactoryWrongReturn>()));
    }
}