// Author: Gockner, Simon
// Created: 2021-12-03
// Copyright(c) 2021 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Validation;
using Moq;
using NUnit.Framework;

namespace Test.LightweightIocContainer;

[TestFixture]
public class IocValidatorTest
{
    public interface ITest
    {
            
    }
        
    public interface ITest2
    {
            
    }
        
    [UsedImplicitly]
    public interface IParameter
    {
        bool Method();
    }

    private class Test : ITest
    {
        public Test(IParameter parameter) => parameter.Method();
    }
    
    private class Test2 : ITest2
    {
        public Test2(ITest dependency)
        {
                
        }
    }
        
        
        
    [UsedImplicitly]
    public interface ITestFactory
    {
        ITest Create(IParameter parameter);
    }
        
    [UsedImplicitly]
    public interface ITest2Factory
    {
        ITest2 InvalidCreate();
        ITest2 Create(ITest test);
    }

   
        
    [UsedImplicitly]
    public interface IInvalidFactory
    {
        ITest Create();
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
        public void Install(IRegistrationCollector registration) => registration.Add<ITest, Test>().WithFactory<IInvalidFactory>();
    }
        
    private class InvalidTestClassInstaller : IIocInstaller
    {
        public void Install(IRegistrationCollector registration)
        {
            registration.Add<ITest, Test>().WithFactory<ITestFactory>();
            registration.Add<ITest2, Test2>().WithFactory<ITest2Factory>();
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

        Mock<IParameter> parameterMock = new();
        parameterMock.Setup(p => p.Method()).Returns(true);

        validator.AddParameter<ITest, IParameter>(parameterMock.Object);
            
        validator.Validate();
            
        parameterMock.Verify(p => p.Method(), Times.Never);
    }
        
           
    [Test]
    public void TestValidateWithInvalidParameterWithFactory()
    {
        IocContainer iocContainer = new();
        iocContainer.Install(new InvalidTestClassInstaller());
            
        IocValidator validator = new(iocContainer);

        Mock<IParameter> parameterMock = new();
        validator.AddParameter<ITest, IParameter>(parameterMock.Object);
            
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => validator.Validate());

        if (aggregateException?.InnerExceptions[0] is not NoMatchingConstructorFoundException noMatchingConstructorFoundException)
        {
            Assert.Fail($"{nameof(aggregateException.InnerExceptions)} is not of type {nameof(NoMatchingConstructorFoundException)}");
            return;
        }
            
        if (noMatchingConstructorFoundException.InnerExceptions[0] is not ConstructorNotMatchingException iTest2CtorNotMatchingException)
        {
            Assert.Fail($"{nameof(aggregateException.InnerExceptions)} is not of type {nameof(ConstructorNotMatchingException)}");
            return;
        }
            
        Assert.IsInstanceOf<DirectResolveWithRegisteredFactoryNotAllowed>(iTest2CtorNotMatchingException.InnerExceptions[0]);
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

    private void AssertNoMatchingConstructorFoundForType<T>(AggregateException aggregateException)
    {
        Exception exception = aggregateException?.InnerExceptions[0];
        if (exception is NoMatchingConstructorFoundException noMatchingConstructorFoundException)
            Assert.AreEqual(typeof(T), noMatchingConstructorFoundException.Type);
        else
            Assert.Fail($"Exception is no {nameof(NoMatchingConstructorFoundException)}, actual type: {exception?.GetType()}");
    }
}