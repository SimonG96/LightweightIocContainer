// Author: Gockner, Simon
// Created: 2019-11-05
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System;
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
        
        [UsedImplicitly]
        private class ATwoCtor : IA
        {
            public ATwoCtor(IB b) => Console.WriteLine("A with args");
            public ATwoCtor() => Console.WriteLine("A without args");
        }
        
        [UsedImplicitly]
        private class BTwoCtor : IB
        {
            public BTwoCtor(IA a) => Console.WriteLine("B with args");
            public BTwoCtor() => Console.WriteLine("B without args");
        }


        private IocContainer _iocContainer;

        [SetUp]
        public void SetUp() => _iocContainer = new IocContainer();

        [TearDown]
        public void TearDown() => _iocContainer.Dispose();

        [Test]
        public void TestCircularDependencies()
        {
            _iocContainer.Register(r => r.Add<IFoo, Foo>());
            _iocContainer.Register(r => r.Add<IBar, Bar>());

            NoMatchingConstructorFoundException noMatchingConstructorFoundException = Assert.Throws<NoMatchingConstructorFoundException>(() => _iocContainer.Resolve<IFoo>());
            ConstructorNotMatchingException fooConstructorNotMatchingException = (ConstructorNotMatchingException) noMatchingConstructorFoundException?.InnerExceptions[0];
            ConstructorNotMatchingException barConstructorNotMatchingException = (ConstructorNotMatchingException) fooConstructorNotMatchingException?.InnerExceptions[0];
            NoMatchingConstructorFoundException noMatchingBarConstructorFoundException = (NoMatchingConstructorFoundException) barConstructorNotMatchingException?.InnerExceptions[0]; 
            ConstructorNotMatchingException secondFooConstructorNotMatchingException = (ConstructorNotMatchingException) noMatchingBarConstructorFoundException?.InnerExceptions[0];
            ConstructorNotMatchingException secondBarConstructorNotMatchingException = (ConstructorNotMatchingException) secondFooConstructorNotMatchingException?.InnerExceptions[0];
            
            CircularDependencyException exception = (CircularDependencyException) secondBarConstructorNotMatchingException?.InnerExceptions[0];
            
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
        public void TestNonCircularDependencies()
        {
            _iocContainer.Register(r => r.Add<IA, A>());
            _iocContainer.Register(r => r.Add<IB, B>());
            _iocContainer.Register(r => r.Add<IC, C>());

            IA a = _iocContainer.Resolve<IA>();
            Assert.IsNotNull(a);
        }

        [Test]
        public void TestRecursionWithParam()
        {
            _iocContainer.Register(r => r.Add<IFoo, Foo>());
            _iocContainer.Register(r => r.Add<IBar, Bar>());

            Assert.DoesNotThrow(() => _iocContainer.Resolve<IFoo>(new Mock<IBar>().Object));
            Assert.DoesNotThrow(() => _iocContainer.Resolve<IBar>(new Mock<IFoo>().Object));
        }

        [Test]
        public void TestNonCircularCrossDependencies()
        {
            _iocContainer.Register(r => r.Add<IA, ATwoCtor>());
            _iocContainer.Register(r => r.Add<IB, BTwoCtor>());

            Assert.DoesNotThrow(() => _iocContainer.Resolve<IA>());
            Assert.DoesNotThrow(() => _iocContainer.Resolve<IB>());
        }
    }
}