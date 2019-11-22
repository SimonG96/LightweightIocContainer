// Author: Gockner, Simon
// Created: 2019-06-06
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Registrations;
using Moq;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    public class RegistrationBaseTest
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
            RegistrationFactory registrationFactory = new RegistrationFactory(new Mock<IIocContainer>().Object);
            IRegistrationBase<ITest> testRegistration = registrationFactory.Register<ITest, Test>(Lifestyle.Transient).OnCreate(t => t.DoSomething());

            ITest test = new Test();
            
            Assert.Throws<Exception>(() => testRegistration.OnCreateAction(test));
        }
    }
}