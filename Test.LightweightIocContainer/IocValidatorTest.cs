// Author: Gockner, Simon
// Created: 2021-12-03
// Copyright(c) 2021 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Interfaces;
using LightweightIocContainer.Interfaces.Installers;
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
        
        private class TestInstaller : IIocInstaller
        {
            public void Install(IIocContainer container) => container.Register<ITest, Test>().WithFactory<ITestFactory>();
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
    }
}