// Author: Gockner, Simon
// Created: 2022-09-07
// Copyright(c) 2022 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Exceptions;
using NUnit.Framework;

namespace Test.LightweightIocContainer;

public class IgnoreConstructorAttributeTest
{
    [AttributeUsage(AttributeTargets.Constructor)]
    private class IgnoreAttribute : Attribute
    {
        
    }

    [AttributeUsage(AttributeTargets.Class)]
    private class InvalidIgnoreAttribute : Attribute
    {
        
    }
    
    [UsedImplicitly]
    public interface ITest
    {
            
    }
    
    [UsedImplicitly]
    private class TestIgnoreCtor : ITest
    {
        [UsedImplicitly]
        private readonly string _name;
        public TestIgnoreCtor(string name) => _name = name;

        [Ignore]
        public TestIgnoreCtor() => throw new Exception();
    }
    
    [UsedImplicitly]
    private class TestOnlyIgnoredCtor : ITest
    {
        [Ignore]
        public TestOnlyIgnoredCtor() => throw new Exception();
        
        [Ignore]
        public TestOnlyIgnoredCtor(string name) => throw new Exception();
    }

    [Test]
    public void TestRegisterValidIgnoreAttribute()
    {
        IocContainer container = new();
        container.Register(r => r.Add<ITest, TestIgnoreCtor>());
        
        container.RegisterIgnoreConstructorAttribute<IgnoreAttribute>();

        ITest test = container.Resolve<ITest>("name");
        Assert.That(test, Is.InstanceOf<TestIgnoreCtor>());
    }

    [Test]
    public void TestRegisterInvalidIgnoreAttribute()
    {
        IocContainer container = new();
        Assert.Throws<InvalidIgnoreConstructorAttributeException<InvalidIgnoreAttribute>>(() => container.RegisterIgnoreConstructorAttribute<InvalidIgnoreAttribute>());
    }

    [Test]
    public void TestResolveWithOnlyIgnoredCtors()
    {
        IocContainer container = new();
        container.Register(r => r.Add<ITest, TestOnlyIgnoredCtor>());
        
        container.RegisterIgnoreConstructorAttribute<IgnoreAttribute>();

        Assert.Throws<NoPublicConstructorFoundException>(() => container.Resolve<ITest>("name"));
    }
}