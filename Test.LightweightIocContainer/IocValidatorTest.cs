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
            public Test(IParameter parameter) => parameter.Method();
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
                Assert.Fail($"Exception is no NoMatchingConstructorFoundException, actual type: {exception?.GetType()}");
        }
    }
}