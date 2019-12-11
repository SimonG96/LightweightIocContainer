// Author: Gockner, Simon
// Created: 2019-12-11
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    public class ActionExtensionTest
    {
        #region TestClasses

        private interface IBar
        {
            void Throw();
        }

        private interface IFoo : IBar
        {

        }

        private class Foo : IFoo
        {
            public void Throw()
            {
                throw new Exception();
            }
        }

        #endregion TestClasses

        [Test]
        public void TestConvert()
        {
            Action<IBar> barAction = bar => bar.Throw();
            Action<IFoo> action = barAction.Convert<IFoo, IBar>();

            Assert.Throws<Exception>(() => action(new Foo()));
        }

        [Test]
        public void TestConvertActionNull()
        {
            Action<IBar> barAction = null;
            Assert.Null(barAction.Convert<IFoo, IBar>());
        }
    }
}