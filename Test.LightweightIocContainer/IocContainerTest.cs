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
            ITest CreateTest(string name = null);

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

            public TestConstructor(Test test, string name = null)
            {

            }
        }

        public class MultitonScope
        {

        }

        #endregion TestClasses


        private IIocContainer _iocContainer;

        [SetUp]
        public void SetUp()
        {
            _iocContainer = new IocContainer();
        }


        [TearDown]
        public void TearDown()
        {
            _iocContainer.Dispose();
        }


        [Test]
        public void TestInstall()
        {
            Mock<IIocInstaller> installerMock = new Mock<IIocInstaller>();
            IIocContainer returnedContainer = _iocContainer.Install(installerMock.Object);

            installerMock.Verify(m => m.Install(It.IsAny<IIocContainer>()), Times.Once);

            Assert.AreEqual(_iocContainer, returnedContainer);
        }

        [Test]
        public void TestInstallMultiple()
        {
            Mock<IIocInstaller> installer1Mock = new Mock<IIocInstaller>();
            Mock<IIocInstaller> installer2Mock = new Mock<IIocInstaller>();
            Mock<IIocInstaller> installer3Mock = new Mock<IIocInstaller>();

            IIocContainer returnedContainer = _iocContainer.Install(installer1Mock.Object, installer2Mock.Object, installer3Mock.Object);

            installer1Mock.Verify(m => m.Install(It.IsAny<IIocContainer>()), Times.Once);
            installer2Mock.Verify(m => m.Install(It.IsAny<IIocContainer>()), Times.Once);
            installer3Mock.Verify(m => m.Install(It.IsAny<IIocContainer>()), Times.Once);

            Assert.AreEqual(_iocContainer, returnedContainer);
        }

        [Test]
        public void TestRegister()
        {
            Assert.DoesNotThrow(() => _iocContainer.Register(RegistrationFactory.Register(typeof(ITest), typeof(Test))));
        }

        [Test]
        public void TestRegisterTypeWithoutInterface()
        {
            Assert.DoesNotThrow(() => _iocContainer.Register(RegistrationFactory.Register(typeof(Test))));
        }

        [Test]
        public void TestRegisterMultiton()
        {
            Assert.DoesNotThrow(() => _iocContainer.Register(RegistrationFactory.Register(typeof(ITest), typeof(Test), typeof(MultitonScope))));
        }

        [Test]
        public void TestRegisterFactory()
        {
            Assert.DoesNotThrow(() => _iocContainer.Register(RegistrationFactory.RegisterFactory(typeof(ITestFactory), _iocContainer)));
        }

        [Test]
        public void TestRegisterMultiple()
        {
            _iocContainer.Register(RegistrationFactory.Register<ITest, Test>());
            MultipleRegistrationException exception = Assert.Throws<MultipleRegistrationException>(() => _iocContainer.Register(RegistrationFactory.Register<ITest, TestConstructor>()));
            Assert.AreEqual(typeof(ITest), exception.Type);
        }

        [Test]
        public void TestRegisterInterfaceWithoutImplementation()
        {
            Assert.Throws<InvalidRegistrationException>(() => _iocContainer.Register(RegistrationFactory.Register<ITest>()));
            Assert.Throws<InvalidRegistrationException>(() => _iocContainer.Register(RegistrationFactory.Register(typeof(ITest))));
        }

        [Test]
        public void TestRegisterFactoryWithoutCreate()
        {
            Assert.Throws<InvalidFactoryRegistrationException>(() => _iocContainer.Register(RegistrationFactory.RegisterFactory<ITestFactoryNoCreate>(_iocContainer)));
        }

        [Test]
        public void TestRegisterFactoryClearMultitonsNonGeneric()
        {
            Assert.Throws<IllegalAbstractMethodCreationException>(() => _iocContainer.Register(RegistrationFactory.RegisterFactory<ITestFactoryNonGenericClear>(_iocContainer)));
        }

        [Test]
        public void TestResolveNotRegistered()
        {
            TypeNotRegisteredException exception = Assert.Throws<TypeNotRegisteredException>(() => _iocContainer.Resolve<ITest>());
            Assert.AreEqual(typeof(ITest), exception.Type);
        }

        [Test]
        public void TestResolve()
        {
            _iocContainer.Register(RegistrationFactory.Register<ITest, Test>());

            ITest resolvedTest = _iocContainer.Resolve<ITest>();

            Assert.IsInstanceOf<Test>(resolvedTest);
        }

        [Test]
        public void TestResolveWithoutInterface()
        {
            _iocContainer.Register(RegistrationFactory.Register<Test>());

            Test resolvedTest = _iocContainer.Resolve<Test>();

            Assert.IsInstanceOf<Test>(resolvedTest);
        }

        [Test]
        public void TestResolveWithParams()
        {
            _iocContainer.Register(RegistrationFactory.Register<ITest, TestConstructor>());

            ITest resolvedTest = _iocContainer.Resolve<ITest>("Test", new Test());

            Assert.IsInstanceOf<TestConstructor>(resolvedTest);
        }

        [Test]
        public void TestResolveWithMissingParam()
        {
            _iocContainer.Register(RegistrationFactory.Register<ITest, TestConstructor>());
            _iocContainer.Register(RegistrationFactory.Register<Test, Test>()); //this registration is abnormal and should only be used in unit tests

            ITest resolvedTest = _iocContainer.Resolve<ITest>("Test");

            Assert.IsInstanceOf<TestConstructor>(resolvedTest);
        }

        [Test]
        public void TestResolveReflection()
        {
            _iocContainer.Register(RegistrationFactory.Register<ITest, Test>());

            object resolvedTest = _iocContainer.Resolve(typeof(ITest), null);

            Assert.IsInstanceOf<Test>(resolvedTest);
        }

        [Test]
        public void TestResolveSingleton()
        {
            _iocContainer.Register(RegistrationFactory.Register<ITest, Test>(Lifestyle.Singleton));

            ITest resolvedTest = _iocContainer.Resolve<ITest>();
            ITest secondResolvedTest = _iocContainer.Resolve<ITest>();

            Assert.AreEqual(resolvedTest, secondResolvedTest);
        }

        [Test]
        public void TestResolveMultiton()
        {
            _iocContainer.Register(RegistrationFactory.Register<ITest, Test, MultitonScope>());

            MultitonScope scope1 = new MultitonScope();
            MultitonScope scope2 = new MultitonScope();

            ITest resolvedTest1 = _iocContainer.Resolve<ITest>(scope1);
            ITest resolvedTest2 = _iocContainer.Resolve<ITest>(scope1);
            ITest resolvedTest3 = _iocContainer.Resolve<ITest>(scope2);

            Assert.AreSame(resolvedTest1, resolvedTest2);
            Assert.AreNotSame(resolvedTest1, resolvedTest3);
            Assert.AreNotSame(resolvedTest2, resolvedTest3);
        }

        [Test]
        public void TestResolveMultitonNoArgs()
        {
            _iocContainer.Register(RegistrationFactory.Register<ITest, Test, MultitonScope>());

            MultitonResolveException exception = Assert.Throws<MultitonResolveException>(() => _iocContainer.Resolve<ITest>());
            Assert.AreEqual(typeof(ITest), exception.Type);
        }

        [Test]
        public void TestResolveMultitonWrongArgs()
        {
            _iocContainer.Register(RegistrationFactory.Register<ITest, Test, MultitonScope>());

            MultitonResolveException exception = Assert.Throws<MultitonResolveException>(() => _iocContainer.Resolve<ITest>(new object()));
            Assert.AreEqual(typeof(ITest), exception.Type);
        }

        [Test]
        public void TestResolveTransient()
        {
            _iocContainer.Register(RegistrationFactory.Register<ITest, Test>());

            ITest resolvedTest = _iocContainer.Resolve<ITest>();
            ITest secondResolvedTest = _iocContainer.Resolve<ITest>();

            Assert.AreNotEqual(resolvedTest, secondResolvedTest);
        }

        [Test]
        public void TestResolveFactory()
        {
            _iocContainer.Register(RegistrationFactory.Register<ITest, Test>());
            _iocContainer.Register(RegistrationFactory.RegisterFactory<ITestFactory>(_iocContainer));

            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();

            Assert.IsInstanceOf<ITestFactory>(testFactory);
        }

        [Test]
        public void TestResolveFromFactory()
        {
            _iocContainer.Register(RegistrationFactory.Register<ITest, Test>());
            _iocContainer.Register(RegistrationFactory.RegisterFactory<ITestFactory>(_iocContainer));

            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.Create();

            Assert.IsInstanceOf<Test>(createdTest);
        }

        [Test]
        public void TestResolveFromFactoryWithParams()
        {
            _iocContainer.Register(RegistrationFactory.Register<ITest, TestConstructor>());
            _iocContainer.Register(RegistrationFactory.Register<Test, Test>()); //this registration is abnormal and should only be used in unit tests
            _iocContainer.Register(RegistrationFactory.RegisterFactory<ITestFactory>(_iocContainer));

            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.Create("Test");

            Assert.IsInstanceOf<TestConstructor>(createdTest);
        }

        [Test]
        public void TestResolveFromFactoryWithDefaultParamCreate()
        {
            _iocContainer.Register(RegistrationFactory.Register<ITest, TestConstructor>());
            _iocContainer.Register(RegistrationFactory.Register<Test, Test>()); //this registration is abnormal and should only be used in unit tests
            _iocContainer.Register(RegistrationFactory.RegisterFactory<ITestFactory>(_iocContainer));

            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.CreateTest();

            Assert.IsInstanceOf<TestConstructor>(createdTest);
        }

        [Test]
        public void TestResolveFromFactoryWithDefaultParamCtor()
        {
            _iocContainer.Register(RegistrationFactory.Register<ITest, TestConstructor>());
            _iocContainer.Register(RegistrationFactory.Register<Test, Test>()); //this registration is abnormal and should only be used in unit tests
            _iocContainer.Register(RegistrationFactory.RegisterFactory<ITestFactory>(_iocContainer));

            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.Create();

            Assert.IsInstanceOf<TestConstructor>(createdTest);
        }

        [Test]
        public void TestResolveMultitonFromFactory()
        {
            _iocContainer.Register(RegistrationFactory.Register<ITest, Test, MultitonScope>());
            _iocContainer.Register(RegistrationFactory.RegisterFactory<ITestFactory>(_iocContainer));

            MultitonScope scope1 = new MultitonScope();
            MultitonScope scope2 = new MultitonScope();

            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();

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
            _iocContainer.Register(RegistrationFactory.Register<ITest, Test, MultitonScope>());
            _iocContainer.Register(RegistrationFactory.RegisterFactory<ITestFactory>(_iocContainer));

            MultitonScope scope1 = new MultitonScope();
            MultitonScope scope2 = new MultitonScope();

            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();

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

        [Test]
        public void TestIsTypeRegistered()
        {
            Assert.False(_iocContainer.IsTypeRegistered<ITest>());

            _iocContainer.Register(RegistrationFactory.Register<ITest, Test>());
            Assert.True(_iocContainer.IsTypeRegistered<ITest>());

            _iocContainer.Register(RegistrationFactory.Register<Test>());
            Assert.True(_iocContainer.IsTypeRegistered<Test>());
        }
    }
}