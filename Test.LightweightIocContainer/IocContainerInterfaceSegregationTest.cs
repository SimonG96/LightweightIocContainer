// Author: Simon Gockner
// Created: 2019-12-07
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using LightweightIocContainer;
using LightweightIocContainer.Interfaces;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    public class IocContainerInterfaceSegregationTest
    {
        #region TestClasses

        private interface IBar
        {
            void ThrowBar();
        }

        private interface IFoo
        {
            void ThrowFoo();
        }

        private class Foo : IFoo, IBar
        {
            public void ThrowFoo()
            {
                throw new Exception("Foo");
            }

            public void ThrowBar()
            {
                throw new Exception("Bar");
            }
        }

        #endregion TestClasses

        private IIocContainer _container;

        [SetUp]
        public void SetUp()
        {
            _container = new IocContainer();
        }

        [TearDown]
        public void TearDown()
        {
            _container.Dispose();
        }

        [Test]
        public void TestRegistrationOnCreate()
        {
            _container.Register<IBar, IFoo, Foo>().OnCreate(bar => bar.ThrowBar(), foo => foo.ThrowFoo());

            Exception fooException = Assert.Throws<Exception>(() => _container.Resolve<IFoo>());
            Assert.AreEqual("Foo", fooException.Message);
            
            Exception barException = Assert.Throws<Exception>(() => _container.Resolve<IBar>());
            Assert.AreEqual("Bar", barException.Message);
        }

        [Test]
        public void TestResolveTransient()
        {
            _container.Register<IBar, IFoo, Foo>();
            IFoo foo = _container.Resolve<IFoo>();
            IBar bar = _container.Resolve<IBar>();

            Assert.IsInstanceOf<Foo>(foo);
            Assert.IsInstanceOf<Foo>(bar);
        }

        [Test]
        public void TestResolveSingleton()
        {
            _container.Register<IBar, IFoo, Foo>(Lifestyle.Singleton);
            IFoo foo = _container.Resolve<IFoo>();
            IBar bar = _container.Resolve<IBar>();

            Assert.IsInstanceOf<Foo>(foo);
            Assert.IsInstanceOf<Foo>(bar);
            Assert.AreEqual(foo, bar);
            Assert.AreSame(foo, bar);
        }
    }
}