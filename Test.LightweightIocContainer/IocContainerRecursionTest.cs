// Author: Gockner, Simon
// Created: 2019-11-05
// Copyright(c) 2019 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Exceptions;
using Moq;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    public class IocContainerRecursionTest
    {
        [UsedImplicitly]
        public interface IFoo
        {

        }

        [UsedImplicitly]
        public interface IBar
        {

        }

        [UsedImplicitly]
        private class Foo : IFoo
        {
            public Foo(IBar bar)
            {
            }
        }

        [UsedImplicitly]
        private class Bar : IBar
        {
            public Bar(IFoo foo)
            {
            }
        }

        [UsedImplicitly]
        public interface IA
        {
            
        }

        [UsedImplicitly]
        public interface IB
        {
            
        }

        [UsedImplicitly]
        public interface IC
        {
            
        }

        [UsedImplicitly]
        private class A : IA
        {
            public A(IB b, IC c)
            {
                
            }
        }

        [UsedImplicitly]
        private class B : IB
        {
            public B(IC c)
            {
                
            }
        }

        [UsedImplicitly]
        private class C : IC
        {

        }


        private IocContainer _iocContainer;

        [SetUp]
        public void SetUp() => _iocContainer = new IocContainer();

        [TearDown]
        public void TearDown() => _iocContainer.Dispose();

        [Test]
        public void TestCircularDependencies()
        {
            _iocContainer.Register<IFoo, Foo>();
            _iocContainer.Register<IBar, Bar>();

            CircularDependencyException exception = Assert.Throws<CircularDependencyException>(() => _iocContainer.Resolve<IFoo>());
            Assert.AreEqual(typeof(IFoo), exception?.ResolvingType);
            Assert.AreEqual(2, exception.ResolveStack.Count);

            string message = $"Circular dependency has been detected when trying to resolve `{typeof(IFoo)}`.\n" +
                             "Resolve stack that resulted in the circular dependency:\n" +
                             $"\t`{typeof(IFoo)}` resolved as dependency of\n" +
                             $"\t`{typeof(IBar)}` resolved as dependency of\n" +
                             $"\t`{typeof(IFoo)}` which is the root type being resolved.";

            Assert.AreEqual(message, exception.Message);
        }

        [Test]
        public void TestCircularDependencyExceptionNoStack()
        {
            CircularDependencyException circularDependencyException = new(typeof(IFoo), null);
            string message = $"Circular dependency has been detected when trying to resolve `{typeof(IFoo)}`.\n";
            Assert.AreEqual(message, circularDependencyException.Message);
        }

        [Test]
        public void TestNonCircularDependencies()
        {
            _iocContainer.Register<IA, A>();
            _iocContainer.Register<IB, B>();
            _iocContainer.Register<IC, C>();

            IA a = _iocContainer.Resolve<IA>();
            Assert.IsNotNull(a);
        }

        [Test]
        public void TestRecursionWithParam()
        {
            _iocContainer.Register<IFoo, Foo>();
            _iocContainer.Register<IBar, Bar>();

            Assert.DoesNotThrow(() => _iocContainer.Resolve<IFoo>(new Mock<IBar>().Object));
            Assert.DoesNotThrow(() => _iocContainer.Resolve<IBar>(new Mock<IFoo>().Object));
        }
    }
}