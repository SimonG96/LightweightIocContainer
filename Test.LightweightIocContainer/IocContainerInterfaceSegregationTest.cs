// Author: Simon Gockner
// Created: 2019-12-07
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Interfaces;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    public class IocContainerInterfaceSegregationTest
    {
        private interface IBar
        {
            
        }

        private interface IFoo
        {
            void ThrowFoo();
        }
        private interface IAnotherBar
        {
            
        }

        private interface IAnotherFoo
        {
            
        }

        private interface IAnotherOne
        {
            
        }

        [UsedImplicitly]
        private class Foo : IFoo, IBar, IAnotherFoo, IAnotherBar, IAnotherOne
        {
            public void ThrowFoo() => throw new Exception("Foo");
        }


        private IIocContainer _container;

        [SetUp]
        public void SetUp() => _container = new IocContainer();

        [TearDown]
        public void TearDown() => _container.Dispose();

        [Test]
        public void TestRegistrationOnCreate2()
        {
            _container.Register<IBar, IFoo, Foo>().OnCreate(foo => foo.ThrowFoo());

            Exception fooException = Assert.Throws<Exception>(() => _container.Resolve<IFoo>());
            Assert.AreEqual("Foo", fooException?.Message);
            
            Exception barException = Assert.Throws<Exception>(() => _container.Resolve<IBar>());
            Assert.AreEqual("Foo", barException?.Message);
        }

        [Test]
        public void TestRegistrationOnCreate3()
        {
            _container.Register<IBar, IFoo, IAnotherFoo, Foo>().OnCreate(foo => foo.ThrowFoo());

            Exception fooException = Assert.Throws<Exception>(() => _container.Resolve<IFoo>());
            Assert.AreEqual("Foo", fooException?.Message);

            Exception barException = Assert.Throws<Exception>(() => _container.Resolve<IBar>());
            Assert.AreEqual("Foo", barException?.Message);

            Exception anotherFooException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherFoo>());
            Assert.AreEqual("Foo", anotherFooException?.Message);
        }

        [Test]
        public void TestRegistrationOnCreate4()
        {
            _container.Register<IBar, IFoo, IAnotherFoo, IAnotherBar, Foo>().OnCreate(foo => foo.ThrowFoo());

            Exception fooException = Assert.Throws<Exception>(() => _container.Resolve<IFoo>());
            Assert.AreEqual("Foo", fooException?.Message);

            Exception barException = Assert.Throws<Exception>(() => _container.Resolve<IBar>());
            Assert.AreEqual("Foo", barException?.Message);

            Exception anotherFooException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherFoo>());
            Assert.AreEqual("Foo", anotherFooException?.Message);

            Exception anotherBarException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherBar>());
            Assert.AreEqual("Foo", anotherBarException?.Message);
        }

        [Test]
        public void TestRegistrationOnCreate5()
        {
            _container.Register<IBar, IFoo, IAnotherFoo, IAnotherBar, IAnotherOne, Foo>().OnCreate(foo => foo.ThrowFoo());

            Exception fooException = Assert.Throws<Exception>(() => _container.Resolve<IFoo>());
            Assert.AreEqual("Foo", fooException?.Message);

            Exception barException = Assert.Throws<Exception>(() => _container.Resolve<IBar>());
            Assert.AreEqual("Foo", barException?.Message);

            Exception anotherFooException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherFoo>());
            Assert.AreEqual("Foo", anotherFooException?.Message);

            Exception anotherBarException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherBar>());
            Assert.AreEqual("Foo", anotherBarException?.Message);

            Exception anotherOneException = Assert.Throws<Exception>(() => _container.Resolve<IAnotherOne>());
            Assert.AreEqual("Foo", anotherOneException?.Message);
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