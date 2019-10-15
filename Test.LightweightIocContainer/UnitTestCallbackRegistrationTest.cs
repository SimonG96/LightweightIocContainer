// Author: Gockner, Simon
// Created: 2019-10-15
// Copyright(c) 2019 SimonG. All Rights Reserved.

using LightweightIocContainer;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Registrations;
using Moq;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    public class UnitTestCallbackRegistrationTest
    {
        private interface ITest
        {
            int Count { get; }
            string Name { get; }
            bool Testing { get; }
        }

        private class Test : ITest
        {
            public Test()
            {
                Count = -1;
                Name = "Test";
                Testing = false;
            }

            public Test(int count, string name, bool testing)
            {
                Count = count;
                Name = name;
                Testing = testing;
            }

            public int Count { get; }
            public string Name { get; }
            public bool Testing { get; }
        }

        [Test]
        public void TestUnitTestResolveCallback()
        {
            RegistrationFactory registrationFactory = new RegistrationFactory(new Mock<IIocContainer>().Object);

            ITest test = new Test();
            IUnitTestCallbackRegistration<ITest> testCallbackRegistration = registrationFactory.RegisterUnitTestCallback(delegate { return test; });

            Assert.AreEqual(test, testCallbackRegistration.UnitTestResolveCallback.Invoke());
        }

        [Test]
        public void TestUnitTestResolveCallbackWithParams()
        {
            RegistrationFactory registrationFactory = new RegistrationFactory(new Mock<IIocContainer>().Object);
            IUnitTestCallbackRegistration<ITest> testCallbackRegistration =
                registrationFactory.RegisterUnitTestCallback(new ResolveCallback<ITest>(parameters => new Test((int) parameters[0], (string) parameters[1], (bool) parameters[2])));

            ITest test = testCallbackRegistration.UnitTestResolveCallback.Invoke(4, "SomeTest", true);

            Assert.AreEqual(4, test.Count);
            Assert.AreEqual("SomeTest", test.Name);
            Assert.True(test.Testing);
        }
    }
}