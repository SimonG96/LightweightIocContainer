// Author: Simon Gockner
// Created: 2019-12-10
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Registrations;
using Moq;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    public class OnCreateTest
    {
        #region TestClasses

        private interface ITest
        {
            void DoSomething();
        }

        private interface IFoo
        {

        }

        private interface IBar
        {

        }

        private class Test : ITest
        {
            public void DoSomething()
            {
                throw new Exception();
            }
        }

        [UsedImplicitly]
        private class Foo : IFoo
        {
            public Foo(IBar bar, ITest test)
            {

            }
        }

        private class Bar : IBar
        {

        }

        #endregion

        [Test]
        public void TestOnCreate()
        {
            RegistrationFactory registrationFactory = new RegistrationFactory(new Mock<IIocContainer>().Object);
            IDefaultRegistration<ITest> testRegistration = (IDefaultRegistration<ITest>) registrationFactory.Register<ITest, Test>(Lifestyle.Transient).OnCreate(t => t.DoSomething());

            Test test = new Test();

            Assert.Throws<Exception>(() => testRegistration.OnCreateAction(test));
        }
    }
}