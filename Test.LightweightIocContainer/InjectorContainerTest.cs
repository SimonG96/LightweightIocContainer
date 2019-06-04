using LightweightIocContainer;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Registrations;
using Moq;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    public class InjectorContainerTest
    {
        #region TestClasses
        //some of the test classes have to be public to allow the implementation of the factory

        public interface ITest
        {

        }

        public interface ITestFactory
        {
            ITest Create();
            ITest Create(string name);
        }

        private class Test : ITest
        {

        }

        private class TestConstructor : ITest
        {
            public TestConstructor(string name, Test test)
            {

            }
        }

        #endregion


        [Test]
        public void TestInstall()
        {
            Mock<IInjectorInstaller> installerMock = new Mock<IInjectorInstaller>();
            
            IInjectorContainer injectorContainer = new InjectorContainer();
            IInjectorContainer returnedContainer = injectorContainer.Install(installerMock.Object);

            installerMock.Verify(m => m.Install(It.IsAny<IInjectorContainer>()), Times.Once);

            Assert.AreEqual(injectorContainer, returnedContainer);
        }

        [Test]
        public void TestInstallMultiple()
        {
            Mock<IInjectorInstaller> installer1Mock = new Mock<IInjectorInstaller>();
            Mock<IInjectorInstaller> installer2Mock = new Mock<IInjectorInstaller>();
            Mock<IInjectorInstaller> installer3Mock = new Mock<IInjectorInstaller>();

            IInjectorContainer injectorContainer = new InjectorContainer();
            IInjectorContainer returnedContainer = injectorContainer.Install(installer1Mock.Object, installer2Mock.Object, installer3Mock.Object);

            installer1Mock.Verify(m => m.Install(It.IsAny<IInjectorContainer>()), Times.Once);
            installer2Mock.Verify(m => m.Install(It.IsAny<IInjectorContainer>()), Times.Once);
            installer3Mock.Verify(m => m.Install(It.IsAny<IInjectorContainer>()), Times.Once);

            Assert.AreEqual(injectorContainer, returnedContainer);
        }

        [Test]
        public void TestResolveNotRegistered()
        {
            IInjectorContainer injectorContainer = new InjectorContainer();

            Assert.Throws<TypeNotRegisteredException>(() => injectorContainer.Resolve<ITest>());
        }

        [Test]
        public void TestResolve()
        {
            IInjectorContainer injectorContainer = new InjectorContainer();
            injectorContainer.Register(RegistrationFactory.Register<ITest, Test>());

            ITest resolvedTest = injectorContainer.Resolve<ITest>();

            Assert.IsInstanceOf<Test>(resolvedTest);
        }

        [Test]
        public void TestResolveWithParams()
        {
            IInjectorContainer injectorContainer = new InjectorContainer();
            injectorContainer.Register(RegistrationFactory.Register<ITest, TestConstructor>());

            ITest resolvedTest = injectorContainer.Resolve<ITest>("Test", new Test());

            Assert.IsInstanceOf<TestConstructor>(resolvedTest);
        }

        [Test]
        public void TestResolveWithMissingParam()
        {
            IInjectorContainer injectorContainer = new InjectorContainer();
            injectorContainer.Register(RegistrationFactory.Register<ITest, TestConstructor>());
            injectorContainer.Register(RegistrationFactory.Register<Test, Test>()); //this registration is abnormal and should only be used in unit tests

            ITest resolvedTest = injectorContainer.Resolve<ITest>("Test");

            Assert.IsInstanceOf<TestConstructor>(resolvedTest);
        }

        [Test]
        public void TestResolveReflection()
        {
            IInjectorContainer injectorContainer = new InjectorContainer();
            injectorContainer.Register(RegistrationFactory.Register<ITest, Test>());

            object resolvedTest = injectorContainer.Resolve(typeof(ITest), null);

            Assert.IsInstanceOf<Test>(resolvedTest);
        }

        [Test]
        public void TestResolveSingleton()
        {
            IInjectorContainer injectorContainer = new InjectorContainer();
            injectorContainer.Register(RegistrationFactory.Register<ITest, Test>(Lifestyle.Singleton));

            ITest resolvedTest = injectorContainer.Resolve<ITest>();
            ITest secondResolvedTest = injectorContainer.Resolve<ITest>();

            Assert.AreEqual(resolvedTest, secondResolvedTest);
        }

        [Test]
        public void TestResolveTransient()
        {
            IInjectorContainer injectorContainer = new InjectorContainer();
            injectorContainer.Register(RegistrationFactory.Register<ITest, Test>());

            ITest resolvedTest = injectorContainer.Resolve<ITest>();
            ITest secondResolvedTest = injectorContainer.Resolve<ITest>();

            Assert.AreNotEqual(resolvedTest, secondResolvedTest);
        }

        [Test]
        public void TestResolveFactory()
        {
            IInjectorContainer injectorContainer = new InjectorContainer();
            injectorContainer.Register(RegistrationFactory.Register<ITest, Test>());
            injectorContainer.Register(RegistrationFactory.RegisterFactory<ITestFactory>(injectorContainer));

            ITestFactory testFactory = injectorContainer.Resolve<ITestFactory>();

            Assert.IsInstanceOf<ITestFactory>(testFactory);
        }

        [Test]
        public void TestResolveFromFactory()
        {
            IInjectorContainer injectorContainer = new InjectorContainer();
            injectorContainer.Register(RegistrationFactory.Register<ITest, Test>());
            injectorContainer.Register(RegistrationFactory.RegisterFactory<ITestFactory>(injectorContainer));

            ITestFactory testFactory = injectorContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.Create();

            Assert.IsInstanceOf<Test>(createdTest);
        }

        [Test]
        public void TestResolveFromFactoryWithParams()
        {
            IInjectorContainer injectorContainer = new InjectorContainer();
            injectorContainer.Register(RegistrationFactory.Register<ITest, TestConstructor>());
            injectorContainer.Register(RegistrationFactory.Register<Test, Test>()); //this registration is abnormal and should only be used in unit tests
            injectorContainer.Register(RegistrationFactory.RegisterFactory<ITestFactory>(injectorContainer));

            ITestFactory testFactory = injectorContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.Create("Test");

            Assert.IsInstanceOf<TestConstructor>(createdTest);
        }
    }
}