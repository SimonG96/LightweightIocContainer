// Author: Gockner, Simon
// Created: 2021-12-03
// Copyright(c) 2021 SimonG. All Rights Reserved.

using System;
using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Exceptions;
using LightweightIocContainer.Interfaces.Installers;
using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Validation;
using Moq;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    public class IocValidatorTest
    {
        public interface ITest
        {
            
        }
        
        [UsedImplicitly]
        public interface IParameter
        {
            bool Method();
        }

        private class Test : ITest
        {
            public Test(IParameter parameter) => parameter?.Method();
        }
        
        [UsedImplicitly]
        public interface ITestFactory
        {
            ITest Create(IParameter parameter);
        }
        
        [UsedImplicitly]
        public interface IInvalidFactory
        {
            ITest Create();
        }
        
        private class TestInstaller : IIocInstaller
        {
            public void Install(IRegistrationCollector registration) => registration.Add<ITest, Test>().WithFactory<ITestFactory>();
        }
        
        private class InvalidTestInstaller : IIocInstaller
        {
            public void Install(IRegistrationCollector registration) => registration.Add<ITest, Test>().WithFactory<IInvalidFactory>();
        }
        
        [Test]
        public void TestValidate()
        {
            IocContainer iocContainer = new();
            iocContainer.Install(new TestInstaller());
            
            IocValidator validator = new(iocContainer);
            
            validator.Validate();
        }
        
        [Test]
        public void TestValidateWithParameter()
        {
            IocContainer iocContainer = new();
            iocContainer.Install(new TestInstaller());
            
            IocValidator validator = new(iocContainer);

            Mock<IParameter> parameterMock = new();
            parameterMock.Setup(p => p.Method()).Returns(true);

            validator.AddParameter<ITest, IParameter>(parameterMock.Object);
            
            validator.Validate();
            
            parameterMock.Verify(p => p.Method(), Times.Once);
        }
        
        [Test]
        public void TestValidateInvalidFactory()
        {
            IocContainer iocContainer = new();
            iocContainer.Install(new InvalidTestInstaller());
            
            IocValidator validator = new(iocContainer);
            
            AggregateException aggregateException = Assert.Throws<AggregateException>(() => validator.Validate());
            
            Exception exception =  aggregateException?.InnerExceptions[0];
            Assert.IsInstanceOf<NoMatchingConstructorFoundException>(exception);
            
            NoMatchingConstructorFoundException noMatchingConstructorFoundException = (NoMatchingConstructorFoundException) exception;
            Assert.AreEqual(typeof(Test), noMatchingConstructorFoundException?.Type);
        }
    }
}