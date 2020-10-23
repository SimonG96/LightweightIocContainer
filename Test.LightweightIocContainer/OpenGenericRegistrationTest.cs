// Author: Simon Gockner
// Created: 2020-09-18
// Copyright(c) 2020 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    public class OpenGenericRegistrationTest
    {
        private IIocContainer _iocContainer;
        
        [UsedImplicitly]
        public interface ITest<T>
        {
            
        }
        
        [UsedImplicitly]
        public class Test<T> : ITest<T>
        {
            
        }

        [SetUp]
        public void SetUp() => _iocContainer = new IocContainer();

        [TearDown]
        public void TearDown() => _iocContainer.Dispose();

        [Test]
        public void TestRegisterOpenGenericType()
        {
            _iocContainer.RegisterOpenGenerics(typeof(ITest<>), typeof(Test<>));

            ITest<int> test = _iocContainer.Resolve<ITest<int>>();
            Assert.NotNull(test);
        }

        [Test]
        public void TestRegisterOpenGenericTypeAsSingleton()
        {
            _iocContainer.RegisterOpenGenerics(typeof(ITest<>), typeof(Test<>), Lifestyle.Singleton);

            ITest<int> test = _iocContainer.Resolve<ITest<int>>();
            Assert.NotNull(test);

            ITest<int> secondTest = _iocContainer.Resolve<ITest<int>>();
            Assert.NotNull(secondTest);
            
            Assert.AreEqual(test, secondTest);
            Assert.AreSame(test, secondTest);
        }
        
        [Test]
        public void TestRegisterOpenGenericTypeAsMultitonThrowsException() 
            => Assert.Throws<InvalidRegistrationException>(() => _iocContainer.RegisterOpenGenerics(typeof(ITest<>), typeof(Test<>), Lifestyle.Multiton));

        [Test]
        public void TestRegisterNonOpenGenericTypeWithOpenGenericsFunctionThrowsException() 
            => Assert.Throws<InvalidRegistrationException>(() => _iocContainer.RegisterOpenGenerics(typeof(int), typeof(int)));
    }
}