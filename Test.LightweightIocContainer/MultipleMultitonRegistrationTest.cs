// Author: Gockner, Simon
// Created: 2020-11-19
// Copyright(c) 2020 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Interfaces;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    public class MultipleMultitonRegistrationTest
    {
        private IIocContainer _iocContainer;
        
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
        public class Test : ITest
        {
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
            _iocContainer.RegisterMultiton<IProvider, ITest, Test, MultitonScope>();

            MultitonScope scope = new();
            
            ITest test = _iocContainer.Resolve<ITest>(scope);
            Assert.NotNull(test);

            IProvider provider = _iocContainer.Resolve<IProvider>(scope);
            Assert.NotNull(provider);
            Assert.AreEqual(test, provider);
            Assert.AreSame(test, provider);
        }

        [Test]
        public void TestRegisterAndResolveMultipleMultitonRegistrationWithDifferentScope()
        {
            _iocContainer.RegisterMultiton<IProvider, ITest, Test, MultitonScope>();

            MultitonScope scope = new();
            MultitonScope differentScope = new();
            
            ITest test = _iocContainer.Resolve<ITest>(scope);
            Assert.NotNull(test);

            IProvider provider = _iocContainer.Resolve<IProvider>(differentScope);
            Assert.NotNull(provider);
            
            Assert.AreNotEqual(test, provider);
            Assert.AreNotSame(test, provider);
        }

        [Test]
        public void TestMultipleMultitonRegistrationOnCreate()
        {
            _iocContainer.RegisterMultiton<IProvider, ITest, Test, MultitonScope>().OnCreate(t => t.DoSomething(1));
            
            MultitonScope scope = new();
            
            ITest test = _iocContainer.Resolve<ITest>(scope);
            Assert.NotNull(test);
            Assert.AreEqual(1, test.Number);

            IProvider provider = _iocContainer.Resolve<IProvider>(scope);
            Assert.NotNull(provider);
            Assert.AreEqual(1, provider.Number);
            
            Assert.AreEqual(test, provider);
            Assert.AreSame(test, provider);
        }
    }
}