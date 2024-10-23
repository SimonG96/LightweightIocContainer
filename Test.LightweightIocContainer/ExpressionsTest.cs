// Author: Simon.Gockner
// Created: 2024-04-24
// Copyright(c) 2024 SimonG. All Rights Reserved.

using System.Reflection;
using LightweightIocContainer;
using NUnit.Framework;

namespace Test.LightweightIocContainer;

public class ExpressionsTest
{
    public class TestNoCtor;
    public class TestWithCtor(string name);
    
    [Test]
    public void TestCreateExpressionWithDefaultCtor()
    {
        Expressions expressions = new();

        ConstructorInfo defaultCtor = typeof(TestNoCtor).GetConstructors().First();
        TestNoCtor testNoCtor = expressions.Create<TestNoCtor>(defaultCtor);
        
        Assert.That(testNoCtor, Is.InstanceOf<TestNoCtor>());
    }
    
    [Test]
    public void TestCreateExpressionWithParamCtor()
    {
        Expressions expressions = new();

        ConstructorInfo paramCtor = typeof(TestWithCtor).GetConstructors().First();
        TestWithCtor testWithCtor = expressions.Create<TestWithCtor>(paramCtor, "TestName");
        
        Assert.That(testWithCtor, Is.InstanceOf<TestWithCtor>());
    }
}