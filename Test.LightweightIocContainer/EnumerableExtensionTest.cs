// Author: Gockner, Simon
// Created: 2019-07-03
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using LightweightIocContainer;
using NSubstitute;
using NUnit.Framework;

namespace Test.LightweightIocContainer;

[TestFixture]
public class EnumerableExtensionTest
{
    private class ListObject
    {
        public int Index { get; set; }
    }

    private class Given : ListObject
    {

    }

    [UsedImplicitly]
    public interface ITest
    {
        void DoSomething();
    }


    [Test]
    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
    public void TestFirstOrGivenNoPredicateEmpty()
    {
        List<ListObject> list = [];
        Assert.That(list.FirstOrGiven<ListObject, Given>(), Is.InstanceOf<Given>());
    }

    [Test]
    public void TestFirstOrGivenNoPredicate()
    {
        List<ListObject> list =
        [
            new() { Index = 0 },
            new() { Index = 1 },
            new() { Index = 2 },
            new() { Index = 3 }
        ];

        ListObject listObject = list.FirstOrGiven<ListObject, Given>();
    
        Assert.That(listObject, Is.Not.InstanceOf<Given>());
        Assert.That(listObject.Index, Is.EqualTo(0));
    }

    [Test]
    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
    public void TestFirstOrGivenPredicateEmpty()
    {
        List<ListObject> list = [];
        Assert.That(list.FirstOrGiven<ListObject, Given>(o => o.Index == 2), Is.InstanceOf<Given>());
    }

    [Test]
    public void TestFirstOrGivenPredicate()
    {
        List<ListObject> list =
        [
            new() { Index = 0 },
            new() { Index = 1 },
            new() { Index = 2 },
            new() { Index = 3 }
        ];

        ListObject listObject = list.FirstOrGiven<ListObject, Given>(o => o.Index == 2);

        Assert.That(listObject, Is.Not.InstanceOf<Given>());
        Assert.That(listObject.Index, Is.EqualTo(2));
    }

    [Test]
    public void TestForEach()
    {
        ITest test1 = Substitute.For<ITest>();
        ITest test2 = Substitute.For<ITest>();
        ITest test3 = Substitute.For<ITest>();
        ITest test4 = Substitute.For<ITest>();

        IEnumerable<ITest> enumerable = [test1, test2, test3, test4];
            
        enumerable.ForEach(t => t.DoSomething());
            
        test1.Received(1).DoSomething();
        test2.Received(1).DoSomething();
        test3.Received(1).DoSomething();
        test4.Received(1).DoSomething();
    }
}