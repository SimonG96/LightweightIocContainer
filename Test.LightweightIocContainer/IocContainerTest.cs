using LightweightIocContainer;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Registrations;
using Moq;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    public class IocContainerTest
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
            Mock<IIocInstaller> installerMock = new Mock<IIocInstaller>();
            
            IIocContainer iocContainer = new IocContainer();
            IIocContainer returnedContainer = iocContainer.Install(installerMock.Object);

            installerMock.Verify(m => m.Install(It.IsAny<IIocContainer>()), Times.Once);

            Assert.AreEqual(iocContainer, returnedContainer);
        }

        [Test]
        public void TestInstallMultiple()
        {
            Mock<IIocInstaller> installer1Mock = new Mock<IIocInstaller>();
            Mock<IIocInstaller> installer2Mock = new Mock<IIocInstaller>();
            Mock<IIocInstaller> installer3Mock = new Mock<IIocInstaller>();

            IIocContainer iocContainer = new IocContainer();
            IIocContainer returnedContainer = iocContainer.Install(installer1Mock.Object, installer2Mock.Object, installer3Mock.Object);

            installer1Mock.Verify(m => m.Install(It.IsAny<IIocContainer>()), Times.Once);
            installer2Mock.Verify(m => m.Install(It.IsAny<IIocContainer>()), Times.Once);
            installer3Mock.Verify(m => m.Install(It.IsAny<IIocContainer>()), Times.Once);

            Assert.AreEqual(iocContainer, returnedContainer);
        }

        [Test]
        public void TestResolveNotRegistered()
        {
            IIocContainer iocContainer = new IocContainer();

            Assert.Throws<TypeNotRegisteredException>(() => iocContainer.Resolve<ITest>());
        }

        [Test]
        public void TestResolve()
        {
            IIocContainer iocContainer = new IocContainer();
            iocContainer.Register(RegistrationFactory.Register<ITest, Test>());

            ITest resolvedTest = iocContainer.Resolve<ITest>();

            Assert.IsInstanceOf<Test>(resolvedTest);
        }

        [Test]
        public void TestResolveWithParams()
        {
            IIocContainer iocContainer = new IocContainer();
            iocContainer.Register(RegistrationFactory.Register<ITest, TestConstructor>());

            ITest resolvedTest = iocContainer.Resolve<ITest>("Test", new Test());

            Assert.IsInstanceOf<TestConstructor>(resolvedTest);
        }

        [Test]
        public void TestResolveWithMissingParam()
        {
            IIocContainer iocContainer = new IocContainer();
            iocContainer.Register(RegistrationFactory.Register<ITest, TestConstructor>());
            iocContainer.Register(RegistrationFactory.Register<Test, Test>()); //this registration is abnormal and should only be used in unit tests

            ITest resolvedTest = iocContainer.Resolve<ITest>("Test");

            Assert.IsInstanceOf<TestConstructor>(resolvedTest);
        }

        [Test]
        public void TestResolveReflection()
        {
            IIocContainer iocContainer = new IocContainer();
            iocContainer.Register(RegistrationFactory.Register<ITest, Test>());

            object resolvedTest = iocContainer.Resolve(typeof(ITest), null);

            Assert.IsInstanceOf<Test>(resolvedTest);
        }

        [Test]
        public void TestResolveSingleton()
        {
            IIocContainer iocContainer = new IocContainer();
            iocContainer.Register(RegistrationFactory.Register<ITest, Test>(Lifestyle.Singleton));

            ITest resolvedTest = iocContainer.Resolve<ITest>();
            ITest secondResolvedTest = iocContainer.Resolve<ITest>();

            Assert.AreEqual(resolvedTest, secondResolvedTest);
        }

        [Test]
        public void TestResolveTransient()
        {
            IIocContainer iocContainer = new IocContainer();
            iocContainer.Register(RegistrationFactory.Register<ITest, Test>());

            ITest resolvedTest = iocContainer.Resolve<ITest>();
            ITest secondResolvedTest = iocContainer.Resolve<ITest>();

            Assert.AreNotEqual(resolvedTest, secondResolvedTest);
        }

        [Test]
        public void TestResolveFactory()
        {
            IIocContainer iocContainer = new IocContainer();
            iocContainer.Register(RegistrationFactory.Register<ITest, Test>());
            iocContainer.Register(RegistrationFactory.RegisterFactory<ITestFactory>(iocContainer));

            ITestFactory testFactory = iocContainer.Resolve<ITestFactory>();

            Assert.IsInstanceOf<ITestFactory>(testFactory);
        }

        [Test]
        public void TestResolveFromFactory()
        {
            IIocContainer iocContainer = new IocContainer();
            iocContainer.Register(RegistrationFactory.Register<ITest, Test>());
            iocContainer.Register(RegistrationFactory.RegisterFactory<ITestFactory>(iocContainer));

            ITestFactory testFactory = iocContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.Create();

            Assert.IsInstanceOf<Test>(createdTest);
        }

        [Test]
        public void TestResolveFromFactoryWithParams()
        {
            IIocContainer iocContainer = new IocContainer();
            iocContainer.Register(RegistrationFactory.Register<ITest, TestConstructor>());
            iocContainer.Register(RegistrationFactory.Register<Test, Test>()); //this registration is abnormal and should only be used in unit tests
            iocContainer.Register(RegistrationFactory.RegisterFactory<ITestFactory>(iocContainer));

            ITestFactory testFactory = iocContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.Create("Test");

            Assert.IsInstanceOf<TestConstructor>(createdTest);
        }
    }
}