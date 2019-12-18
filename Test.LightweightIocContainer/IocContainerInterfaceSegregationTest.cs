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
        private interface IAnotherBar
        {
            void ThrowAnotherBar();
        }

        private interface IAnotherFoo
        {
            void ThrowAnotherFoo();
        }

        private interface IAnotherOne
        {
            void ThrowAnotherOne();
        }

        private class Foo : IFoo, IBar, IAnotherFoo, IAnotherBar, IAnotherOne
        {
            public void ThrowFoo()
            {
                throw new Exception("Foo");
            }

            public void ThrowBar()
            {
                throw new Exception("Bar");
            }

            public void ThrowAnotherFoo()
            {
                throw new Exception("AnotherFoo");
            }

            public void ThrowAnotherBar()
            {
                throw new Exception("AnotherBar");
            }

            public void ThrowAnotherOne()
            {
                throw new Exception("AnotherOne");
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
        public void TestRegistrationOnCreate2()
        {
            _container.Register<IBar, IFoo, Foo>().OnCreate(bar => bar.ThrowBar(), foo => foo.ThrowFoo());

            Exception fooException = Assert.Throws<Exception>(() => _container.Resolve<IFoo>());
            Assert.AreEqual("Foo", fooException.Message);
            
            Exception barException = Assert.Throws<Exception>(() => _container.Resolve<IBar>());
            Assert.AreEqual("Bar", barException.Message);
        }

        [Test]
        public void TestRegistrationOnCreate3()
        {
            _container.Register<IBar, IFoo, IAnotherFoo, Foo>().OnCreate(bar => bar.ThrowBar(), foo => foo.ThrowFoo(), anotherFoo => anotherFoo.ThrowAnotherFoo());

            Exception fooException = Assert.Throws<Exception>(() => _container.Resolve<IFoo>());
            Assert.AreEqual("Foo", fooException.Message);

            Exception barException = Assert.Throws<Exception>(() => _container.Resolve<IBar>());
            Assert.AreEqual("Bar", barException.Message);

            Exception anotherFooException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherFoo>());
            Assert.AreEqual("AnotherFoo", anotherFooException.Message);
        }

        [Test]
        public void TestRegistrationOnCreate4()
        {
            _container.Register<IBar, IFoo, IAnotherFoo, IAnotherBar, Foo>().OnCreate(bar => bar.ThrowBar(), foo => foo.ThrowFoo(), anotherFoo => anotherFoo.ThrowAnotherFoo(), anotherBar => anotherBar.ThrowAnotherBar());

            Exception fooException = Assert.Throws<Exception>(() => _container.Resolve<IFoo>());
            Assert.AreEqual("Foo", fooException.Message);

            Exception barException = Assert.Throws<Exception>(() => _container.Resolve<IBar>());
            Assert.AreEqual("Bar", barException.Message);

            Exception anotherFooException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherFoo>());
            Assert.AreEqual("AnotherFoo", anotherFooException.Message);

            Exception anotherBarException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherBar>());
            Assert.AreEqual("AnotherBar", anotherBarException.Message);
        }

        [Test]
        public void TestRegistrationOnCreate5()
        {
            _container.Register<IBar, IFoo, IAnotherFoo, IAnotherBar, IAnotherOne, Foo>().OnCreate(bar => bar.ThrowBar(), foo => foo.ThrowFoo(), anotherFoo => anotherFoo.ThrowAnotherFoo(), anotherBar => anotherBar.ThrowAnotherBar(), anotherOne => anotherOne.ThrowAnotherOne());

            Exception fooException = Assert.Throws<Exception>(() => _container.Resolve<IFoo>());
            Assert.AreEqual("Foo", fooException.Message);

            Exception barException = Assert.Throws<Exception>(() => _container.Resolve<IBar>());
            Assert.AreEqual("Bar", barException.Message);

            Exception anotherFooException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherFoo>());
            Assert.AreEqual("AnotherFoo", anotherFooException.Message);

            Exception anotherBarException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherBar>());
            Assert.AreEqual("AnotherBar", anotherBarException.Message);

            Exception anotherOneException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherOne>());
            Assert.AreEqual("AnotherOne", anotherOneException.Message);
        }

        [Test]
        public void TestResolveTransient()
        {
            _container.Register<IBar, IFoo, IAnotherFoo, IAnotherBar, IAnotherOne, Foo>();
            IFoo foo = _container.Resolve<IFoo>();
            IBar bar = _container.Resolve<IBar>();
            IAnotherFoo anotherFoo = _container.Resolve<IAnotherFoo>();
            IAnotherBar anotherBar = _container.Resolve<IAnotherBar>();
            IAnotherOne anotherOne = _container.Resolve<IAnotherOne>();

            Assert.IsInstanceOf<Foo>(foo);
            Assert.IsInstanceOf<Foo>(bar);
            Assert.IsInstanceOf<Foo>(anotherFoo);
            Assert.IsInstanceOf<Foo>(anotherBar);
            Assert.IsInstanceOf<Foo>(anotherOne);
        }

        [Test]
        public void TestResolveSingleton()
        {
            _container.Register<IBar, IFoo, IAnotherFoo, IAnotherBar, IAnotherOne, Foo>(Lifestyle.Singleton);
            IFoo foo = _container.Resolve<IFoo>();
            IBar bar = _container.Resolve<IBar>();
            IAnotherFoo anotherFoo = _container.Resolve<IAnotherFoo>();
            IAnotherBar anotherBar = _container.Resolve<IAnotherBar>();
            IAnotherOne anotherOne = _container.Resolve<IAnotherOne>();

            Assert.IsInstanceOf<Foo>(foo);
            Assert.IsInstanceOf<Foo>(bar);
            Assert.IsInstanceOf<Foo>(anotherFoo);
            Assert.IsInstanceOf<Foo>(anotherBar);
            Assert.IsInstanceOf<Foo>(anotherOne);
            
            Assert.AreEqual(foo, bar);
            Assert.AreEqual(foo, anotherFoo);
            Assert.AreEqual(foo, anotherBar);
            Assert.AreEqual(foo, anotherOne);

            Assert.AreSame(foo, bar);
            Assert.AreSame(foo, anotherFoo);
            Assert.AreSame(foo, anotherBar);
            Assert.AreSame(foo, anotherOne);
        }
    }
}