// Author: Simon Gockner
// Created: 2020-09-18
// Copyright(c) 2020 SimonG. All Rights Reserved.

using LightweightIocContainer;
using LightweightIocContainer.Interfaces;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    public class OpenGenericRegistrationTest
    {
        public interface ITest<T>
        {
            
        }
        
        public class Test<T> : ITest<T>
        {
            
        }
        
        [Test]
        public void TestRegisterOpenGenericType()
        {
            IIocContainer iocContainer = new IocContainer();
            iocContainer.Register(typeof(ITest<>), typeof(Test<>));

            ITest<int> test = iocContainer.Resolve<ITest<int>>();
            Assert.NotNull(test);
        }
    }
}