// Author: Gockner, Simon
// Created: 2019-11-22
// Copyright(c) 2019 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using LightweightIocContainer.Interfaces;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
    [TestFixture]
    // ReSharper disable MemberHidesStaticFromOuterClass
    public class IocContainerParameterRegistrationTest
    {
        #region TestClasses

        [UsedImplicitly]
        public interface IA
        {
            IB B { get; }
            IC C { get; }
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
        public interface ID
        {
            IA A { get; }
            IA A2 { get; }
            IB B { get; }
            IC C { get; }
        }

        [UsedImplicitly]
        private class A : IA
        {
            public A(IB b, IC c)
            {
                B = b;
                C = c;
            }

            public IB B { get; }
            public IC C { get; }
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
        private class D : ID
        {
            public D(IA a, IA a2, IB b, IC c)
            {
                A = a;
                A2 = a2;
                B = b;
                C = c;
            }

            public IA A { get; }
            public IA A2 { get; }
            public IB B { get; }
            public IC C { get; }
        }

        #endregion TestClasses

        private IIocContainer _iocContainer;

        [SetUp]
        public void SetUp()
        {
            _iocContainer = new IocContainer();
        }


        [TearDown]
        public void TearDown()
        {
            _iocContainer.Dispose();
        }


        [Test]
        public void TestResolveOnlyRegistrationParameters()
        {
            IC c = new C();
            IB b = new B(c);
            
            _iocContainer.Register<IA, A>().WithParameters(b, c);
            IA a = _iocContainer.Resolve<IA>();
            
            Assert.AreEqual(b, a.B);
            Assert.AreEqual(c, a.C);
        }

        [Test]
        public void TestResolveRegistrationAndResolveParameters()
        {
            IC c = new C();
            IB b = new B(c);

            _iocContainer.Register<IA, A>().WithParameters(b);
            IA a = _iocContainer.Resolve<IA>(c);

            Assert.AreEqual(b, a.B);
            Assert.AreEqual(c, a.C);
        }

        [Test]
        public void TestResolveRegistrationAndResolveParametersMixedOrder()
        {
            IC c = new C();
            IB b = new B(c);
            IA a = new A(b, c);
            IA a2 = new A(b, c);

            _iocContainer.Register<ID, D>().WithParameters((0, a), (2, b), (3, c));
            ID d = _iocContainer.Resolve<ID>(a2);

            Assert.AreEqual(a, d.A);
            Assert.AreEqual(a2, d.A2);
            Assert.AreEqual(b, d.B);
            Assert.AreEqual(c, d.C);
        }

        [Test]
        public void TestResolveRegistrationParametersAndResolvedParameters()
        {
            IC c = new C();
            IB b = new B(c);
            IA a = new A(b, c);
            IA a2 = new A(b, c);

            _iocContainer.Register<ID, D>().WithParameters(a2);
            _iocContainer.Register<IB, B>();
            _iocContainer.Register<IC, C>();

            ID d = _iocContainer.Resolve<ID>(a);

            Assert.AreEqual(a, d.A2);
            Assert.AreEqual(a2, d.A);
        }
    }
}