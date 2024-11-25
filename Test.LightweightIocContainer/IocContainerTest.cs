using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;
using NSubstitute;
using NUnit.Framework;

namespace Test.LightweightIocContainer;

[TestFixture]
public class IocContainerTest
{
    private interface ITest
    {

    }

    private interface IFoo
    {

    }

    private class Test : ITest
    {

    }
    
    private class TestMultiton : ITest
    {
        public TestMultiton(MultitonScope multitonScope)
        {
            
        }
    }
    
    private class TestMultitonIntScope(int scope) : ITest
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
    private class TestWithFoo : ITest
    {
        public TestWithFoo(IFoo testFoo) => TestFoo = testFoo;
        
        [UsedImplicitly]
        public IFoo TestFoo { get; }
    }

    [UsedImplicitly]
    private class Foo : IFoo
    {

    }

    [UsedImplicitly]
    private class FooConstructor : IFoo
    {
        public FooConstructor(string test)
        {
                
        }
    }

    private class MultitonScope
    {

    }


    private IocContainer _iocContainer;

    [SetUp]
    public void SetUp() => _iocContainer = new IocContainer();

    [TearDown]
    public void TearDown() => _iocContainer.Dispose();


    [Test]
    public void TestInstall()
    {
        IIocInstaller installerMock = Substitute.For<IIocInstaller>();
        IIocContainer returnedContainer = _iocContainer.Install(installerMock);

        installerMock.Received(1).Install(Arg.Any<IRegistrationCollector>());

        Assert.That(returnedContainer, Is.EqualTo(_iocContainer));
    }

    [Test]
    public void TestInstallMultiple()
    {
        IIocInstaller installer1Mock = Substitute.For<IIocInstaller>();
        IIocInstaller installer2Mock = Substitute.For<IIocInstaller>();
        IIocInstaller installer3Mock = Substitute.For<IIocInstaller>();

        IIocContainer returnedContainer = _iocContainer.Install(installer1Mock, installer2Mock, installer3Mock);

        installer1Mock.Received(1).Install(Arg.Any<IRegistrationCollector>());
        installer2Mock.Received(1).Install(Arg.Any<IRegistrationCollector>());
        installer3Mock.Received(1).Install(Arg.Any<IRegistrationCollector>());

        Assert.That(returnedContainer, Is.EqualTo(_iocContainer));
    }

    [Test]
    public void TestRegister() => Assert.DoesNotThrow(() => _iocContainer.Register(r => r.Add<ITest, Test>()));

    [Test]
    public void TestRegisterTypeWithoutInterface() => Assert.DoesNotThrow(() => _iocContainer.Register(r => r.Add<Test>()));

    [Test]
    public void TestRegisterMultiton() => Assert.DoesNotThrow(() => _iocContainer.Register(r => r.AddMultiton<ITest, Test, MultitonScope>()));

    [Test]
    public void TestInvalidMultitonRegistration() => Assert.Throws<InvalidRegistrationException>(() => _iocContainer.Register(r => r.Add<ITest, Test>(Lifestyle.Multiton)));

    [Test]
    public void TestRegisterMultipleDifferent()
    {
        _iocContainer.Register(r => r.Add<ITest, Test>());
        MultipleRegistrationException exception = Assert.Throws<MultipleRegistrationException>(() => _iocContainer.Register(r => r.Add<ITest, TestConstructor>()));
        Assert.That(exception?.Type, Is.EqualTo(typeof(ITest)));
    }
        
    [Test]
    public void TestRegisterMultipleSame()
    {
        _iocContainer.Register(r => r.Add<ITest, Test>());
        Assert.DoesNotThrow(() => _iocContainer.Register(r => r.Add<ITest, Test>()));
    }

    [Test]
    public void TestRegisterMultipleSameWithParameters()
    {
        _iocContainer.Register(r => r.Add<ITest, Test>().WithParameters("test", 1, new Foo()));
        Assert.DoesNotThrow(() => _iocContainer.Register(r => r.Add<ITest, Test>().WithParameters("test", 1, new Foo())));
    }
        
    [Test]
    public void TestResolveNotRegistered()
    {
        TypeNotRegisteredException exception = Assert.Throws<TypeNotRegisteredException>(() => _iocContainer.Resolve<ITest>());
        Assert.That(exception?.Type, Is.EqualTo(typeof(ITest)));
    }

    [Test]
    public void TestResolve()
    {
        _iocContainer.Register(r => r.Add<ITest, Test>());

        ITest resolvedTest = _iocContainer.Resolve<ITest>();

        Assert.That(resolvedTest, Is.InstanceOf<Test>());
    }

    [Test]
    public void TestResolveWithoutInterface()
    {
        _iocContainer.Register(r => r.Add<Test>());

        Test resolvedTest = _iocContainer.Resolve<Test>();

        Assert.That(resolvedTest, Is.InstanceOf<Test>());
    }

    [Test]
    public void TestResolveInterfaceWithoutImplementation()
    {
        _iocContainer.Register(r => r.Add<ITest>());
        Assert.Throws<InvalidRegistrationException>(() => _iocContainer.Resolve<ITest>());
    }
        
    [Test]
    public void TestResolveImplementationRegisteredWithInterface()
    {
        _iocContainer.Register(r => r.Add<ITest, Test>());

        Test resolvedTest = _iocContainer.Resolve<Test>();
            
        Assert.That(resolvedTest, Is.InstanceOf<Test>());
    }

    [Test]
    public void TestResolveWithParams()
    {
        _iocContainer.Register(r => r.Add<ITest, TestConstructor>());

        ITest resolvedTest = _iocContainer.Resolve<ITest>("Test", new Test());
        
        Assert.That(resolvedTest, Is.InstanceOf<TestConstructor>());
    }

    [Test]
    public void TestResolveWithMissingParam()
    {
        _iocContainer.Register(r => r.Add<ITest, TestConstructor>());
        _iocContainer.Register(r => r.Add<Test, Test>()); //this registration is abnormal and should only be used in unit tests

        ITest resolvedTest = _iocContainer.Resolve<ITest>("Test");

        Assert.That(resolvedTest, Is.InstanceOf<TestConstructor>());
    }

    [Test]
    public void TestResolveSingleton()
    {
        _iocContainer.Register(r => r.Add<ITest, Test>(Lifestyle.Singleton));

        ITest resolvedTest = _iocContainer.Resolve<ITest>();
        ITest secondResolvedTest = _iocContainer.Resolve<ITest>();

        Assert.That(secondResolvedTest, Is.EqualTo(resolvedTest));
    }

    [Test]
    public void TestResolveMultiton()
    {
        _iocContainer.Register(r => r.AddMultiton<ITest, TestMultiton, MultitonScope>());

        MultitonScope scope1 = new();
        MultitonScope scope2 = new();

        ITest resolvedTest1 = _iocContainer.Resolve<ITest>(scope1);
        ITest resolvedTest2 = _iocContainer.Resolve<ITest>(scope1);
        ITest resolvedTest3 = _iocContainer.Resolve<ITest>(scope2);

        Assert.That(resolvedTest1, Is.SameAs(resolvedTest2));
        Assert.That(resolvedTest1, Is.Not.SameAs(resolvedTest3));
        Assert.That(resolvedTest2, Is.Not.SameAs(resolvedTest3));
    }
    
    [Test]
    public void TestResolveMultitonIntScope()
    {
        _iocContainer.Register(r => r.AddMultiton<ITest, TestMultitonIntScope, int>());

        ITest resolvedTest1 = _iocContainer.Resolve<ITest>(1);
        ITest resolvedTest2 = _iocContainer.Resolve<ITest>(1);
        ITest resolvedTest3 = _iocContainer.Resolve<ITest>(2);

        Assert.That(resolvedTest1, Is.SameAs(resolvedTest2));
        Assert.That(resolvedTest1, Is.Not.SameAs(resolvedTest3));
        Assert.That(resolvedTest2, Is.Not.SameAs(resolvedTest3));
    }

    [Test]
    public void TestResolveMultitonNoArgs()
    {
        _iocContainer.Register(r => r.AddMultiton<ITest, Test, MultitonScope>());

        MultitonResolveException exception = Assert.Throws<MultitonResolveException>(() => _iocContainer.Resolve<ITest>());
        Assert.That(exception?.Type, Is.EqualTo(typeof(ITest)));
    }

    [Test]
    public void TestResolveMultitonWrongArgs()
    {
        _iocContainer.Register(r => r.AddMultiton<ITest, Test, MultitonScope>());

        MultitonResolveException exception = Assert.Throws<MultitonResolveException>(() => _iocContainer.Resolve<ITest>(new object()));
        Assert.That(exception?.Type, Is.EqualTo(typeof(ITest)));
    }

    [Test]
    public void TestResolveTransient()
    {
        _iocContainer.Register(r => r.Add<ITest, Test>());

        ITest resolvedTest = _iocContainer.Resolve<ITest>();
        ITest secondResolvedTest = _iocContainer.Resolve<ITest>();

        Assert.That(secondResolvedTest, Is.Not.EqualTo(resolvedTest));
    }

    [Test]
    public void TestResolveNoMatchingConstructor()
    {
        _iocContainer.Register(r => r.Add<ITest, TestConstructor>());
        NoMatchingConstructorFoundException exception = Assert.Throws<NoMatchingConstructorFoundException>(() => _iocContainer.Resolve<ITest>());
        Assert.That(exception?.Type, Is.EqualTo(typeof(TestConstructor)));
    }

    [Test]
    public void TestResolveNoMatchingConstructorNotThrownWrongly()
    {
        _iocContainer.Register(r => r.Add<ITest, TestMultipleConstructors>());
        Assert.DoesNotThrow(() => _iocContainer.Resolve<ITest>("Name"));
    }

    [Test]
    public void TestResolvePrivateConstructor()
    {
        _iocContainer.Register(r => r.Add<ITest, TestPrivateConstructor>());
        NoPublicConstructorFoundException exception = Assert.Throws<NoPublicConstructorFoundException>(() => _iocContainer.Resolve<ITest>());
        Assert.That(exception?.Type, Is.EqualTo(typeof(TestPrivateConstructor)));
    }

    [Test]
    public void TestResolveSingleTypeRegistrationWithFactoryMethod()
    {
        _iocContainer.Register(r => r.Add<IFoo, Foo>());
        _iocContainer.Register(r => r.Add<ITest>().WithFactoryMethod(c => new TestConstructor(c.Resolve<IFoo>(), "someName")));

        ITest test = _iocContainer.Resolve<ITest>();
        
        Assert.That(test, Is.Not.Null);
    }

    [Test]
    public void TestResolveParameterIsRegisteredWithParameters()
    {
        _iocContainer.Register(r => r.Add<ITest, TestConstructor>());
        _iocContainer.Register(r => r.Add<IFoo, FooConstructor>().WithParameters("TestString"));

        ITest test = _iocContainer.Resolve<ITest>("testName");
        
        Assert.That(test, Is.InstanceOf<TestConstructor>());
    }
    
    [Test]
    public void TestResolveParameterWithTwoParameters()
    {
        _iocContainer.Register(r => r.Add<ITest, TestWithFoo>().WithParameters(new Foo()));
        _iocContainer.Register(r => r.Add<IFoo, FooConstructor>(Lifestyle.Singleton).WithParameters("TestString"));

        _iocContainer.Resolve<ITest>();
    }

    [Test]
    public void TestResolveTypeWithToManyParameters()
    {
        _iocContainer.Register(r => r.Add<ITest, TestWithFoo>().WithParameters(new Foo()));
        Assert.Throws<NoMatchingConstructorFoundException>(() => _iocContainer.Resolve<ITest>(new Foo()));
    }

    [Test]
    public void TestIsTypeRegistered()
    {
        Assert.That(_iocContainer.IsTypeRegistered<ITest>(), Is.False);

        _iocContainer.Register(r => r.Add<ITest, Test>());
        Assert.That(_iocContainer.IsTypeRegistered<ITest>(), Is.True);

        _iocContainer.Register(r => r.Add<Test>());
        Assert.That(_iocContainer.IsTypeRegistered<Test>(), Is.True);
    }
}