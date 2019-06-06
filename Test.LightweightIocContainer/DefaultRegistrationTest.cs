// // Author: Gockner, Simon
// // Created: 2019-06-06
// // Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Registrations;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    public class DefaultRegistrationTest
    {
        #region TestClasses

        private interface ITest
        {
            void DoSomething();
        }

        private class Test : ITest
        {
            public void DoSomething()
            {
                throw new Exception();
            }
        }

        #endregion


        [Test]
        public void TestOnCreate()
        {
            IDefaultRegistration<ITest> testRegistration = RegistrationFactory.Register<ITest, Test>().OnCreate(t => t.DoSomething());

            ITest test = new Test();
            
            Assert.Throws<Exception>(() => testRegistration.OnCreateAction(test));
        }
    }
}