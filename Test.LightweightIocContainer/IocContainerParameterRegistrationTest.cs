// Author: Gockner, Simon
// Created: 2019-11-22
// Copyright(c) 2019 SimonG. All Rights Reserved.

using JetBrains.Annotations;
using LightweightIocContainer;
using NUnit.Framework;

namespace Test.LightweightIocContainer;

[TestFixture]
// ReSharper disable MemberHidesStaticFromOuterClass
public class IocContainerParameterRegistrationTest
{
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
        IA A3 { get; }
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
        public D(IA a, IA a2, IA a3, IB b, IC c)
        {
            A = a;
            A2 = a2;
            A3 = a3;
            B = b;
            C = c;
        }

        public IA A { get; }
        public IA A2 { get; }
        public IA A3 { get; }
        public IB B { get; }
        public IC C { get; }
    }

        
    private IocContainer _iocContainer;

    [SetUp]
    public void SetUp() => _iocContainer = new IocContainer();

    [TearDown]
    public void TearDown() => _iocContainer.Dispose();


    [Test]
    public void TestResolveOnlyRegistrationParameters()
    {
        IC c = new C();
        IB b = new B(c);
            
        _iocContainer.Register(r => r.Add<IA, A>().WithParameters(b, c));
        IA a = _iocContainer.Resolve<IA>();
            
        Assert.That(a.B, Is.SameAs(b));
        Assert.That(a.C, Is.SameAs(c));
    }

    [Test]
    public void TestResolveRegistrationAndResolveParameters()
    {
        IC c = new C();
        IB b = new B(c);

        _iocContainer.Register(r => r.Add<IA, A>().WithParameters(b));
        IA a = _iocContainer.Resolve<IA>(c);

        Assert.That(a.B, Is.SameAs(b));
        Assert.That(a.C, Is.SameAs(c));
    }

    [Test]
    public void TestResolveRegistrationAndResolveParametersMixedOrder()
    {
        IC c = new C();
        IB b = new B(c);
        IA a = new A(b, c);
        IA a2 = new A(b, c);
        IA a3 = new A(b, c);

        _iocContainer.Register(r => r.Add<ID, D>().WithParameters((0, a), (2, a3), (3, b), (4, c)));
        ID d = _iocContainer.Resolve<ID>(a2);

        Assert.That(d.A, Is.SameAs(a));
        Assert.That(d.A2, Is.SameAs(a2));
        Assert.That(d.A3, Is.SameAs(a3));
        Assert.That(d.B, Is.SameAs(b));
        Assert.That(d.C, Is.SameAs(c));
    }

    [Test]
    public void TestResolveRegistrationParametersAndResolvedParameters()
    {
        IC c = new C();
        IB b = new B(c);
        IA a = new A(b, c);
        IA a2 = new A(b, c);

        _iocContainer.Register(r => r.Add<ID, D>().WithParameters(a2));
        _iocContainer.Register(r => r.Add<IA, A>());
        _iocContainer.Register(r => r.Add<IB, B>());
        _iocContainer.Register(r => r.Add<IC, C>());

        ID d = _iocContainer.Resolve<ID>(a);

        Assert.That(d.A2, Is.SameAs(a));
        Assert.That(d.A, Is.SameAs(a2));
        Assert.That(d.A3, Is.Not.SameAs(a));
        Assert.That(d.A3, Is.Not.SameAs(a2));
    }
}