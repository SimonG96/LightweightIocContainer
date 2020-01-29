using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Installers;
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
            ITest Create(byte id);

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

        private interface IFoo
        {

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

            public TestConstructor(IFoo foo, string name)
            {

            }
        }

        [UsedImplicitly]
        private class TestPrivateConstructor : ITest
        {
            private TestPrivateConstructor()
            {

            }
        }

        [UsedImplicitly]
        private class TestMultipleConstructors : ITest
        {
            public TestMultipleConstructors(string name, bool success)
            {

            }

            public TestMultipleConstructors(string name)
            {

            }
        }

        [UsedImplicitly]
        private class TestByte : ITest
        {
            [UsedImplicitly]
            private readonly byte _id;

            public TestByte(byte id)
            {
                _id = id;
            }
        }

        [UsedImplicitly]
        private class Foo : IFoo
        {

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
            Assert.DoesNotThrow(() => _iocContainer.Register<ITest, Test>());
        }

        [Test]
        public void TestRegisterTypeWithoutInterface()
        {
            Assert.DoesNotThrow(() => _iocContainer.Register<Test>());
        }

        [Test]
        public void TestRegisterMultiton()
        {
            Assert.DoesNotThrow(() => _iocContainer.RegisterMultiton<ITest, Test, MultitonScope>());
        }

        [Test]
        public void TestInvalidMultitonRegistration()
        {
            Assert.Throws<InvalidRegistrationException>(() => _iocContainer.Register<ITest, Test>(Lifestyle.Multiton));
        }

        [Test]
        public void TestRegisterFactory()
        {
            Assert.DoesNotThrow(() => _iocContainer.RegisterFactory<ITestFactory>());
        }

        [Test]
        public void TestRegisterMultiple()
        {
            _iocContainer.Register<ITest, Test>();
            MultipleRegistrationException exception = Assert.Throws<MultipleRegistrationException>(() => _iocContainer.Register<ITest, TestConstructor>());
            Assert.AreEqual(typeof(ITest), exception.Type);
        }

        [Test]
        public void TestRegisterFactoryWithoutCreate()
        {
            Assert.Throws<InvalidFactoryRegistrationException>(() => _iocContainer.RegisterFactory<ITestFactoryNoCreate>());
        }

        [Test]
        public void TestRegisterFactoryClearMultitonsNonGeneric()
        {
            Assert.Throws<IllegalAbstractMethodCreationException>(() => _iocContainer.RegisterFactory<ITestFactoryNonGenericClear>());
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
            _iocContainer.Register<ITest, Test>();

            ITest resolvedTest = _iocContainer.Resolve<ITest>();

            Assert.IsInstanceOf<Test>(resolvedTest);
        }

        [Test]
        public void TestResolveWithoutInterface()
        {
            _iocContainer.Register<Test>();

            Test resolvedTest = _iocContainer.Resolve<Test>();

            Assert.IsInstanceOf<Test>(resolvedTest);
        }

        [Test]
        public void TestResolveInterfaceWithoutImplementation()
        {
            _iocContainer.Register<ITest>();
            Assert.Throws<InvalidRegistrationException>(() => _iocContainer.Resolve<ITest>());
        }

        [Test]
        public void TestResolveWithParams()
        {
            _iocContainer.Register<ITest, TestConstructor>();

            ITest resolvedTest = _iocContainer.Resolve<ITest>("Test", new Test());

            Assert.IsInstanceOf<TestConstructor>(resolvedTest);
        }

        [Test]
        public void TestResolveWithMissingParam()
        {
            _iocContainer.Register<ITest, TestConstructor>();
            _iocContainer.Register<Test, Test>(); //this registration is abnormal and should only be used in unit tests

            ITest resolvedTest = _iocContainer.Resolve<ITest>("Test");

            Assert.IsInstanceOf<TestConstructor>(resolvedTest);
        }

        [Test]
        public void TestResolveSingleton()
        {
            _iocContainer.Register<ITest, Test>(Lifestyle.Singleton);

            ITest resolvedTest = _iocContainer.Resolve<ITest>();
            ITest secondResolvedTest = _iocContainer.Resolve<ITest>();

            Assert.AreEqual(resolvedTest, secondResolvedTest);
        }

        [Test]
        public void TestResolveMultiton()
        {
            _iocContainer.RegisterMultiton<ITest, Test, MultitonScope>();

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
            _iocContainer.RegisterMultiton<ITest, Test, MultitonScope>();

            MultitonResolveException exception = Assert.Throws<MultitonResolveException>(() => _iocContainer.Resolve<ITest>());
            Assert.AreEqual(typeof(ITest), exception.Type);
        }

        [Test]
        public void TestResolveMultitonWrongArgs()
        {
            _iocContainer.RegisterMultiton<ITest, Test, MultitonScope>();

            MultitonResolveException exception = Assert.Throws<MultitonResolveException>(() => _iocContainer.Resolve<ITest>(new object()));
            Assert.AreEqual(typeof(ITest), exception.Type);
        }

        [Test]
        public void TestResolveTransient()
        {
            _iocContainer.Register<ITest, Test>();

            ITest resolvedTest = _iocContainer.Resolve<ITest>();
            ITest secondResolvedTest = _iocContainer.Resolve<ITest>();

            Assert.AreNotEqual(resolvedTest, secondResolvedTest);
        }

        [Test]
        public void TestResolveNoMatchingConstructor()
        {
            _iocContainer.Register<ITest, TestConstructor>();
            NoMatchingConstructorFoundException exception = Assert.Throws<NoMatchingConstructorFoundException>(() => _iocContainer.Resolve<ITest>());
            Assert.AreEqual(typeof(TestConstructor), exception.Type);
        }

        [Test]
        public void TestResolveNoMatchingConstructorNotThrownWrongly()
        {
            _iocContainer.Register<ITest, TestMultipleConstructors>();
            Assert.DoesNotThrow(() => _iocContainer.Resolve<ITest>("Name"));
        }

        [Test]
        public void TestResolvePrivateConstructor()
        {
            _iocContainer.Register<ITest, TestPrivateConstructor>();
            NoPublicConstructorFoundException exception = Assert.Throws<NoPublicConstructorFoundException>(() => _iocContainer.Resolve<ITest>());
            Assert.AreEqual(typeof(TestPrivateConstructor), exception.Type);
        }

        [Test]
        public void TestResolveFactory()
        {
            _iocContainer.Register<ITest, Test>();
            _iocContainer.RegisterFactory<ITestFactory>();

            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();

            Assert.IsInstanceOf<ITestFactory>(testFactory);
        }

        [Test]
        public void TestResolveFromFactory()
        {
            _iocContainer.Register<ITest, Test>();
            _iocContainer.RegisterFactory<ITestFactory>();

            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.Create();

            Assert.IsInstanceOf<Test>(createdTest);
        }

        [Test]
        public void TestResolveFromFactoryWithParams()
        {
            _iocContainer.Register<ITest, TestConstructor>();
            _iocContainer.Register<Test, Test>(); //this registration is abnormal and should only be used in unit tests
            _iocContainer.RegisterFactory<ITestFactory>();

            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.Create("Test");

            Assert.IsInstanceOf<TestConstructor>(createdTest);
        }

        [Test]
        public void TestResolveFromFactoryWithDefaultParamCreate()
        {
            _iocContainer.Register<ITest, TestConstructor>();
            _iocContainer.Register<Test, Test>(); //this registration is abnormal and should only be used in unit tests
            _iocContainer.RegisterFactory<ITestFactory>();

            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.CreateTest();

            Assert.IsInstanceOf<TestConstructor>(createdTest);
        }

        [Test]
        public void TestResolveFromFactoryWithDefaultParamCtor()
        {
            _iocContainer.Register<ITest, TestConstructor>();
            _iocContainer.Register<Test, Test>(); //this registration is abnormal and should only be used in unit tests
            _iocContainer.RegisterFactory<ITestFactory>();

            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.Create();

            Assert.IsInstanceOf<TestConstructor>(createdTest);
        }

        [Test]
        public void TestResolveFromFactoryWithByte()
        {
            _iocContainer.Register<ITest, TestByte>();
            _iocContainer.RegisterFactory<ITestFactory>();

            ITestFactory testFactory = _iocContainer.Resolve<ITestFactory>();
            ITest createdTest = testFactory.Create(1);

            Assert.IsInstanceOf<TestByte>(createdTest);
        }

        [Test]
        public void TestResolveMultitonFromFactory()
        {
            _iocContainer.RegisterMultiton<ITest, Test, MultitonScope>();
            _iocContainer.RegisterFactory<ITestFactory>();

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
            _iocContainer.RegisterMultiton<ITest, Test, MultitonScope>();
            _iocContainer.RegisterFactory<ITestFactory>();

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
        public void TestResolveSingleTypeRegistrationWithFactoryMethod()
        {
            _iocContainer.Register<IFoo, Foo>();
            _iocContainer.Register<ITest>().WithFactoryMethod(c => new TestConstructor(c.Resolve<IFoo>(), "someName"));

            ITest test = _iocContainer.Resolve<ITest>();
            
            Assert.NotNull(test);
        }

        [Test]
        public void TestIsTypeRegistered()
        {
            Assert.False(_iocContainer.IsTypeRegistered<ITest>());

            _iocContainer.Register<ITest, Test>();
            Assert.True(_iocContainer.IsTypeRegistered<ITest>());

            _iocContainer.Register<Test>();
            Assert.True(_iocContainer.IsTypeRegistered<Test>());
        }
    }
}