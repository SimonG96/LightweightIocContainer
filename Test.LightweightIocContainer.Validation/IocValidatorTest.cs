// Author: Gockner, Simon
// Created: 2021-12-03
// Copyright(c) 2021 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Validation;
using NSubstitute;
using NUnit.Framework;

namespace Test.LightweightIocContainer.Validation;

[TestFixture]
public class IocValidatorTest
{
    public interface ITest;
        
    [UsedImplicitly]
    public interface IParameter
    {
        bool Method();
    }

    private class Test : ITest
    {
        public Test(IParameter parameter) => parameter.Method();
    }
    
    [UsedImplicitly]
    public interface IConstraint;

    [UsedImplicitly]
    public class Constraint : IConstraint;
    
    [UsedImplicitly]
    public interface IGenericTest<T> where T : IConstraint, new();
        
    [UsedImplicitly]
    public class GenericTest<T> : IGenericTest<T> where T : IConstraint, new();
    
    [UsedImplicitly]
    public interface IGenericTestFactory
    {
        IGenericTest<T> Create<T>() where T : IConstraint, new();
    }

    [UsedImplicitly]
    public interface IGenericParameter;
    
    [UsedImplicitly]
    public class GenericParameter : IGenericParameter
    {
        public GenericParameter(IGenericTest<Constraint> test)
        {
            
        }
    }

    [UsedImplicitly]
    private class TestViewModelDontIgnoreDesignTimeCtor : ITest
    {
        public TestViewModelDontIgnoreDesignTimeCtor(string name, IParameter parameter) => parameter.Method();
        
        public TestViewModelDontIgnoreDesignTimeCtor() => throw new Exception();
    }

    private class Parameter : IParameter
    {
        public bool Method() => throw new NotImplementedException();
    }
    
    [UsedImplicitly]
    public interface ITestFactory
    {
        ITest Create(IParameter parameter);
    }
    
    [UsedImplicitly]
    public interface ITestMissingParamFactory
    {
        ITest Create(string name);
    }
    
    [UsedImplicitly]
    public interface IInvalidTestFactory
    {
        ITest Create();
    }
    
    [UsedImplicitly]
    public interface IParameterFactory
    {
        IParameter Create();
    }
        
    private class TestInstallerNoFactory : IIocInstaller
    {
        public void Install(IRegistrationCollector registration) => registration.Add<ITest, Test>();
    }
        
    private class TestInstallerWithFactory : IIocInstaller
    {
        public void Install(IRegistrationCollector registration) => registration.Add<ITest, Test>().WithFactory<ITestFactory>();
    }
        
    private class TestInstallerWithInvalidFactory : IIocInstaller
    {
        public void Install(IRegistrationCollector registration) => registration.Add<ITest, Test>().WithFactory<IInvalidTestFactory>();
    }
    
    private class TestInstallerInvalidFactoryParameterRegisteredWithoutFactory : IIocInstaller
    {
        public void Install(IRegistrationCollector registration)
        {
            registration.Add<ITest, Test>().WithFactory<IInvalidTestFactory>();
            registration.Add<IParameter, Parameter>();
        }
    }
        
    private class TestInstallerInvalidFactoryParameterRegisteredWithFactory : IIocInstaller
    {
        public void Install(IRegistrationCollector registration)
        {
            registration.Add<ITest, Test>().WithFactory<IInvalidTestFactory>();
            registration.Add<IParameter, Parameter>().WithFactory<IParameterFactory>();
        }
    }
    
    private class TestInstallerInvalidFactoryParameterDesignTimeCtorNotIgnoredRegisteredWithFactory : IIocInstaller
    {
        public void Install(IRegistrationCollector registration)
        {
            registration.Add<ITest, Test>().WithFactory<IInvalidTestFactory>();
            registration.Add<IParameter, Parameter>().WithFactory<IParameterFactory>();
        }
    }
    
    private class TestInstallerInvalidFactoryViewModel : IIocInstaller
    {
        public void Install(IRegistrationCollector registration)
        {
            registration.Add<ITest, TestViewModelDontIgnoreDesignTimeCtor>().WithFactory<ITestMissingParamFactory>();
            registration.Add<IParameter, Parameter>().WithFactory<IParameterFactory>();
        }
    }

    [Test]
    public void TestValidateWithoutFactory()
    {
        IocContainer iocContainer = new();
        iocContainer.Install(new TestInstallerNoFactory());
            
        IocValidator validator = new(iocContainer);
            
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => validator.Validate());
            
        AssertNoMatchingConstructorFoundForType<Test>(aggregateException);
    }
        
    [Test]
    public void TestValidateWithFactory()
    {
        IocContainer iocContainer = new();
        iocContainer.Install(new TestInstallerWithFactory());
            
        IocValidator validator = new(iocContainer);
            
        validator.Validate();
    }
        
    [Test]
    public void TestValidateWithParameter()
    {
        IocContainer iocContainer = new();
        iocContainer.Install(new TestInstallerNoFactory());
            
        IocValidator validator = new(iocContainer);

        IParameter parameterMock = Substitute.For<IParameter>();
        parameterMock.Method().Returns(true);

        validator.AddParameter<ITest, IParameter>(parameterMock);
            
        validator.Validate();
            
        parameterMock.DidNotReceive().Method();
    }

    [Test]
    public void TestValidateWithInvalidFactoryParameterWithoutFactory()
    {
        IocContainer iocContainer = new();
        iocContainer.Install(new TestInstallerInvalidFactoryParameterRegisteredWithoutFactory());
            
        IocValidator validator = new(iocContainer);
            
        validator.Validate();
    }
        
    [Test]
    public void TestValidateWithInvalidParameterWithFactory()
    {
        IocContainer iocContainer = new();
        iocContainer.Install(new TestInstallerInvalidFactoryParameterRegisteredWithFactory());
            
        IocValidator validator = new(iocContainer);

        IParameter parameterMock = Substitute.For<IParameter>();
        validator.AddParameter<ITest, IParameter>(parameterMock);
            
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => validator.Validate());

        if (aggregateException?.InnerExceptions[0] is not NoMatchingConstructorFoundException noMatchingConstructorFoundException)
        {
            Assert.Fail($"First element of {nameof(aggregateException.InnerExceptions)} is not of type {nameof(NoMatchingConstructorFoundException)}.");
            return;
        }
            
        if (noMatchingConstructorFoundException.InnerExceptions[0] is not ConstructorNotMatchingException iTest2CtorNotMatchingException)
        {
            Assert.Fail($"First element of {nameof(noMatchingConstructorFoundException.InnerExceptions)} is not of type {nameof(ConstructorNotMatchingException)}.");
            return;
        }
        
        Assert.That(iTest2CtorNotMatchingException.InnerExceptions[0], Is.InstanceOf<DirectResolveWithRegisteredFactoryNotAllowed>());
    }
    
    [Test]
    public void TestValidateViewModelWithInvalidParameterWithFactory()
    {
        IocContainer iocContainer = new();
        iocContainer.Install(new TestInstallerInvalidFactoryViewModel());
            
        IocValidator validator = new(iocContainer);

        AggregateException aggregateException = Assert.Throws<AggregateException>(() => validator.Validate());
        
        if (aggregateException?.InnerExceptions[0] is not NoMatchingConstructorFoundException noMatchingConstructorFoundException)
        {
            Assert.Fail($"First element of {nameof(aggregateException.InnerExceptions)} is not of type {nameof(NoMatchingConstructorFoundException)}.");
            return;
        }
            
        if (noMatchingConstructorFoundException.InnerExceptions[0] is not ConstructorNotMatchingException iTest2CtorNotMatchingException)
        {
            Assert.Fail($"First element of {nameof(noMatchingConstructorFoundException.InnerExceptions)} is not of type {nameof(ConstructorNotMatchingException)}.");
            return;
        }
        
        Assert.That(iTest2CtorNotMatchingException.InnerExceptions[0], Is.InstanceOf<DirectResolveWithRegisteredFactoryNotAllowed>());
    }
    
    [Test]
    public void TestValidateViewModelWithInvalidParameterDesignTimeCtorNotIgnoredWithFactory()
    {
        IocContainer iocContainer = new();
        iocContainer.Install(new TestInstallerInvalidFactoryParameterDesignTimeCtorNotIgnoredRegisteredWithFactory());
            
        IocValidator validator = new(iocContainer);

        AggregateException aggregateException = Assert.Throws<AggregateException>(() => validator.Validate());
        
        if (aggregateException?.InnerExceptions[0] is not NoMatchingConstructorFoundException noMatchingConstructorFoundException)
        {
            Assert.Fail($"First element of {nameof(aggregateException.InnerExceptions)} is not of type {nameof(NoMatchingConstructorFoundException)}.");
            return;
        }
            
        if (noMatchingConstructorFoundException.InnerExceptions[0] is not ConstructorNotMatchingException iTest2CtorNotMatchingException)
        {
            Assert.Fail($"First element of {nameof(noMatchingConstructorFoundException.InnerExceptions)} is not of type {nameof(ConstructorNotMatchingException)}.");
            return;
        }
        
        Assert.That(iTest2CtorNotMatchingException.InnerExceptions[0], Is.InstanceOf<DirectResolveWithRegisteredFactoryNotAllowed>());
    }
        
    [Test]
    public void TestValidateInvalidFactory()
    {
        IocContainer iocContainer = new();
        iocContainer.Install(new TestInstallerWithInvalidFactory());
            
        IocValidator validator = new(iocContainer);
            
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => validator.Validate());
            
        AssertNoMatchingConstructorFoundForType<Test>(aggregateException);
    }

    [Test]
    public void TestValidateFactoryOfOpenGenericType()
    {
        IocContainer iocContainer = new();
        iocContainer.Register(r => r.AddOpenGenerics(typeof(IGenericTest<>), typeof(GenericTest<>)).WithFactory<IGenericTestFactory>());

        IocValidator validator = new(iocContainer);
        validator.Validate();
    }
    
    [Test]
    public void TestValidateOpenGenericTypeWithoutFactory()
    {
        IocContainer iocContainer = new();
        iocContainer.Register(r => r.AddOpenGenerics(typeof(IGenericTest<>), typeof(GenericTest<>)));

        IocValidator validator = new(iocContainer);
        validator.Validate();
    }

    [Test]
    public void TestValidateOpenGenericTypeAsParameter()
    {
        IocContainer iocContainer = new();
        iocContainer.Register(r => r.AddOpenGenerics(typeof(IGenericTest<>), typeof(GenericTest<>)));
        iocContainer.Register(r => r.Add<IGenericParameter, GenericParameter>());

        IocValidator validator = new(iocContainer);
        validator.Validate();
    }

    private void AssertNoMatchingConstructorFoundForType<T>(AggregateException aggregateException)
    {
        Exception exception = aggregateException?.InnerExceptions[0];
        if (exception is NoMatchingConstructorFoundException noMatchingConstructorFoundException)
            Assert.That(noMatchingConstructorFoundException.Type, Is.EqualTo(typeof(T)));
        else
            Assert.Fail($"Exception is no {nameof(NoMatchingConstructorFoundException)}, actual type: {exception?.GetType()}");
    }
}