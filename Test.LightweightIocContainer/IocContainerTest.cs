using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Installers;
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

        [UsedImplicitly]
        public interface ITestFactory
        {
            ITest Create();
            ITest Create(string name);
            ITest Create(MultitonScope scope);

            void ClearMultitonInstance<T>();
        }

        private interface ITestFactoryNoCreate
        {
            
        }

        private interface ITestFactoryNonGenericClear
        {
            ITest Create();

            void ClearMultitonInstance();
        }

        private class Test : ITest
        {

        }

        [UsedImplicitly]
        private class TestConstructor : ITest
        {
            public TestConstructor(string name, Test test)
            {

            }
        }

        public class MultitonScope
        {

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
        public void TestRegister()
        {
            IIocContainer iocContainer = new IocContainer();

            Assert.DoesNotThrow(() => iocContainer.Register(RegistrationFactory.Register(typeof(ITest), typeof(Test))));
            Assert.DoesNotThrow(() => iocContainer.Register(RegistrationFactory.RegisterFactory(typeof(ITestFactory), iocContainer)));
        }

        [Test]
        public void TestRegisterMultiple()
        {
            IIocContainer iocContainer = new IocContainer();

            iocContainer.Register(RegistrationFactory.Register<ITest, Test>());
            MultipleRegistrationException exception = Assert.Throws<MultipleRegistrationException>(() => iocContainer.Register(RegistrationFactory.Register<ITest, TestConstructor>()));
            Assert.AreEqual(typeof(ITest), exception.Type);
        }

        [Test]
        public void TestRegisterFactoryWithoutCreate()
        {
            IIocContainer iocContainer = new IocContainer();

            Assert.Throws<InvalidFactoryRegistrationException>(() => iocContainer.Register(RegistrationFactory.RegisterFactory<ITestFactoryNoCreate>(iocContainer)));
        }

        [Test]
        public void TestRegisterFactoryClearMultitonsNonGeneric()
        {
            IIocContainer iocContainer = new IocContainer();

            Assert.Throws<IllegalAbstractMethodCreationException>(() => iocContainer.Register(RegistrationFactory.RegisterFactory<ITestFactoryNonGenericClear>(iocContainer)));
        }

        [Test]
        public void TestResolveNotRegistered()
        {
            IIocContainer iocContainer = new IocContainer();

            TypeNotRegisteredException exception = Assert.Throws<TypeNotRegisteredException>(() => iocContainer.Resolve<ITest>());
            Assert.AreEqual(typeof(ITest), exception.Type);
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
        public void TestResolveMultiton()
        {
            IIocContainer iocContainer = new IocContainer();
            iocContainer.Register(RegistrationFactory.Register<ITest, Test, MultitonScope>());

            MultitonScope scope1 = new MultitonScope();
            MultitonScope scope2 = new MultitonScope();

            ITest resolvedTest1 = iocContainer.Resolve<ITest>(scope1);
            ITest resolvedTest2 = iocContainer.Resolve<ITest>(scope1);
            ITest resolvedTest3 = iocContainer.Resolve<ITest>(scope2);

            Assert.AreSame(resolvedTest1, resolvedTest2);
            Assert.AreNotSame(resolvedTest1, resolvedTest3);
            Assert.AreNotSame(resolvedTest2, resolvedTest3);
        }

        [Test]
        public void TestResolveMultitonNoArgs()
        {
            IIocContainer iocContainer = new IocContainer();
            iocContainer.Register(RegistrationFactory.Register<ITest, Test, MultitonScope>());

            MultitonResolveException exception = Assert.Throws<MultitonResolveException>(() => iocContainer.Resolve<ITest>());
            Assert.AreEqual(typeof(ITest), exception.Type);
        }

        [Test]
        public void TestResolveMultitonWrongArgs()
        {
            IIocContainer iocContainer = new IocContainer();
            iocContainer.Register(RegistrationFactory.Register<ITest, Test, MultitonScope>());

            MultitonResolveException exception = Assert.Throws<MultitonResolveException>(() => iocContainer.Resolve<ITest>(new object()));
            Assert.AreEqual(typeof(ITest), exception.Type);
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

        [Test]
        public void TestResolveMultitonFromFactory()
        {
            IIocContainer iocContainer = new IocContainer();
            iocContainer.Register(RegistrationFactory.Register<ITest, Test, MultitonScope>());
            iocContainer.Register(RegistrationFactory.RegisterFactory<ITestFactory>(iocContainer));

            MultitonScope scope1 = new MultitonScope();
            MultitonScope scope2 = new MultitonScope();

            ITestFactory testFactory = iocContainer.Resolve<ITestFactory>();

            ITest resolvedTest1 = testFactory.Create(scope1);
            ITest resolvedTest2 = testFactory.Create(scope1);
            ITest resolvedTest3 = testFactory.Create(scope2);

            Assert.AreSame(resolvedTest1, resolvedTest2);
            Assert.AreNotSame(resolvedTest1, resolvedTest3);
            Assert.AreNotSame(resolvedTest2, resolvedTest3);
        }

        [Test]
        public void TestResolveMultitonFromFactoryClearInstances()
        {
            IIocContainer iocContainer = new IocContainer();
            iocContainer.Register(RegistrationFactory.Register<ITest, Test, MultitonScope>());
            iocContainer.Register(RegistrationFactory.RegisterFactory<ITestFactory>(iocContainer));

            MultitonScope scope1 = new MultitonScope();
            MultitonScope scope2 = new MultitonScope();

            ITestFactory testFactory = iocContainer.Resolve<ITestFactory>();

            ITest resolvedTest1 = testFactory.Create(scope1);
            ITest resolvedTest2 = testFactory.Create(scope1);
            ITest resolvedTest3 = testFactory.Create(scope2);

            Assert.AreSame(resolvedTest1, resolvedTest2);
            Assert.AreNotSame(resolvedTest1, resolvedTest3);
            Assert.AreNotSame(resolvedTest2, resolvedTest3);

            testFactory.ClearMultitonInstance<ITest>();

            ITest resolvedTest4 = testFactory.Create(scope1);
            ITest resolvedTest5 = testFactory.Create(scope2);

            Assert.AreNotSame(resolvedTest1, resolvedTest4);
            Assert.AreNotSame(resolvedTest2, resolvedTest4);
            Assert.AreNotSame(resolvedTest3, resolvedTest5);
        }
    }
}