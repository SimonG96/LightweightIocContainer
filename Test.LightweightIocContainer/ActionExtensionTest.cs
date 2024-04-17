// Author: Gockner, Simon
// Created: 2019-12-11
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System.Diagnostics.CodeAnalysis;
using LightweightIocContainer;
using NUnit.Framework;

namespace Test.LightweightIocContainer;

[TestFixture]
public class ActionExtensionTest
{
    private interface IBar
    {
        void Throw();
    }

    private interface IFoo : IBar
    {

    }

    private class Foo : IFoo
    {
        public void Throw() => throw new Exception();
    }
        

    [Test]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public void TestConvert()
    {
        Action<IBar> barAction = bar => bar.Throw();
        Action<IFoo> action = barAction.Convert<IFoo, IBar>();

        Assert.Throws<Exception>(() => action(new Foo()));
    }

    [Test]
    [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
    public void TestConvertActionNull()
    {
        Action<IBar> barAction = null;
        Assert.That(barAction.Convert<IFoo, IBar>(), Is.Null);
    }
}