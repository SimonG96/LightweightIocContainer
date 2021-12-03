// Author: Gockner, Simon
// Created: 2019-06-06
// Copyright(c) 2019 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Registrations;
using Moq;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    public class RegistrationBaseTest
    {
        private interface ITest
        {

        }

        private interface IFoo
        {

        }

        private interface IBar
        {

        }

        private class Test : ITest
        {

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


        [Test]
        public void TestWithParameters()
        {
            RegistrationFactory registrationFactory = new RegistrationFactory(new Mock<IocContainer>().Object);

            IBar bar = new Bar();
            ITest test = new Test();

            IRegistrationBase testRegistration = registrationFactory.Register<IFoo, Foo>(Lifestyle.Transient).WithParameters(bar, test);

            Assert.AreEqual(bar, testRegistration.Parameters[0]);
            Assert.AreEqual(test, testRegistration.Parameters[1]);
        }

        [Test]
        public void TestWithParametersDifferentOrder()
        {
            RegistrationFactory registrationFactory = new RegistrationFactory(new Mock<IocContainer>().Object);

            IBar bar = new Bar();
            ITest test = new Test();

            IRegistrationBase testRegistration = registrationFactory.Register<IFoo, Foo>(Lifestyle.Transient).WithParameters((0, bar), (3, test), (2, "SomeString"));

            Assert.AreEqual(bar, testRegistration.Parameters[0]);
            Assert.IsInstanceOf<InternalResolvePlaceholder>(testRegistration.Parameters[1]);
            Assert.AreEqual("SomeString", testRegistration.Parameters[2]);
            Assert.AreEqual(test, testRegistration.Parameters[3]);
        }

        [Test]
        public void TestWithParametersCalledTwice()
        {
            RegistrationFactory registrationFactory = new RegistrationFactory(new Mock<IocContainer>().Object);
            Assert.Throws<InvalidRegistrationException>(() => registrationFactory.Register<IFoo, Foo>(Lifestyle.Transient).WithParameters(new Bar()).WithParameters(new Test()));
            Assert.Throws<InvalidRegistrationException>(() => registrationFactory.Register<IFoo, Foo>(Lifestyle.Transient).WithParameters((0, new Bar())).WithParameters((1, new Test())));
        }

        [Test]
        public void TestWithParametersNoParametersGiven()
        {
            RegistrationFactory registrationFactory = new RegistrationFactory(new Mock<IocContainer>().Object);
            Assert.Throws<InvalidRegistrationException>(() => registrationFactory.Register<IFoo, Foo>(Lifestyle.Transient).WithParameters((object[])null));
            Assert.Throws<InvalidRegistrationException>(() => registrationFactory.Register<IFoo, Foo>(Lifestyle.Transient).WithParameters(((int index, object parameter)[])null));
        }
    }
}