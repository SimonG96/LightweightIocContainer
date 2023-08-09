// Author: Simon Gockner
// Created: 2019-12-10
// Copyright(c) 2019 SimonG. All Rights Reserved.

using LightweightIocContainer;
using LightweightIocContainer.Interfaces.Registrations;
using LightweightIocContainer.Registrations;
using NSubstitute;
using NUnit.Framework;

namespace Test.LightweightIocContainer;

[TestFixture]
public class OnCreateTest
{
    private interface ITest
    {
        void DoSomething();
    }

    private class Test : ITest
    {
        public void DoSomething() => throw new Exception();
    }

        
    [Test]
    public void TestOnCreate()
    {
        RegistrationFactory registrationFactory = new(Substitute.For<IocContainer>());
        ITypedRegistration<ITest, Test> testRegistration = registrationFactory.Register<ITest, Test>(Lifestyle.Transient).OnCreate(t => t.DoSomething());

        Test test = new();

        Assert.Throws<Exception>(() => testRegistration.OnCreateAction!(test));
    }
}