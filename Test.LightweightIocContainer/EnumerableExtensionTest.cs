// Author: Gockner, Simon
// Created: 2019-07-03
// Copyright(c) 2019 SimonG. All Rights Reserved.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LightweightIocContainer;
using NUnit.Framework;

namespace Test.LightweightIocContainer
{
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
    }
}