// Author: Gockner, Simon
// Created: 2019-07-03
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using LightweightIocContainer;
using Moq;
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
        List<ListObject> list = new();
        Assert.IsInstanceOf<Given>(list.FirstOrGiven<ListObject, Given>());
    }

    [Test]
    public void TestFirstOrGivenNoPredicate()
    {
        List<ListObject> list = new()
        {
            new ListObject {Index = 0},
            new ListObject {Index = 1},
            new ListObject {Index = 2},
            new ListObject {Index = 3}
        };

        ListObject listObject = list.FirstOrGiven<ListObject, Given>();
            
        Assert.IsNotInstanceOf<Given>(listObject);
        Assert.AreEqual(0, listObject.Index);
    }

    [Test]
    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Local")]
    public void TestFirstOrGivenPredicateEmpty()
    {
        List<ListObject> list = new();
        Assert.IsInstanceOf<Given>(list.FirstOrGiven<ListObject, Given>(o => o.Index == 2));
    }

    [Test]
    public void TestFirstOrGivenPredicate()
    {
        List<ListObject> list = new()
        {
            new ListObject {Index = 0},
            new ListObject {Index = 1},
            new ListObject {Index = 2},
            new ListObject {Index = 3}
        };

        ListObject listObject = list.FirstOrGiven<ListObject, Given>(o => o.Index == 2);

        Assert.IsNotInstanceOf<Given>(listObject);
        Assert.AreEqual(2, listObject.Index);
    }

    [Test]
    public void TestForEach()
    {
        Mock<ITest> test1 = new();
        Mock<ITest> test2 = new();
        Mock<ITest> test3 = new();
        Mock<ITest> test4 = new();

        IEnumerable<ITest> enumerable = new[] { test1.Object, test2.Object, test3.Object, test4.Object };
            
        enumerable.ForEach(t => t.DoSomething());
            
        test1.Verify(t => t.DoSomething(), Times.Once);
        test2.Verify(t => t.DoSomething(), Times.Once);
        test3.Verify(t => t.DoSomething(), Times.Once);
        test4.Verify(t => t.DoSomething(), Times.Once);
    }
}