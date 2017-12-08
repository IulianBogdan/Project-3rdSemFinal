using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sem3FinalProject_Code.DBFacade;
using Sem3FinalProject_Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sem3FinalProject_Code.Tests.TestClasses;

namespace Sem3FinalProject_Code.Tests.DBFacade
{
    [TestClass]
    public class TestCachingDBFacade
    {
        [TestMethod]
        public void TestGetItems()
        {
            CachingDBFacade dbFacade = GetTestDBFacade();

            //#Caching.1
            Assert.AreEqual(dbFacade.GetItems("user3").Count, 0);

            //#Caching.2
            Assert.AreEqual(dbFacade.GetItems("user1").Count, 3);
            Assert.IsTrue(dbFacade.GetItems("user1").Contains(new Item("Item1")));
            Assert.IsTrue(dbFacade.GetItems("user1").Contains(new Item("Item2")));
            Assert.IsTrue(dbFacade.GetItems("user1").Contains(new Item("Item3")));
            Assert.AreEqual(dbFacade.GetItems("user2").Count, 1);
            Assert.IsTrue(dbFacade.GetItems("user2").Contains(new Item("Item1")));
        }
        [TestMethod]
        public void TestAddItems()
        {
            CachingDBFacade dbFacade = GetTestDBFacade();

            //#Caching.3
            dbFacade.AddItems(new Item[] { new Item("Item4"), new Item("Item5") }, "user1");
            Assert.AreEqual(dbFacade.GetItems("user1").Count, 5);
            Assert.IsTrue(dbFacade.GetItems("user1").Contains(new Item("Item1")));
            Assert.IsTrue(dbFacade.GetItems("user1").Contains(new Item("Item2")));
            Assert.IsTrue(dbFacade.GetItems("user1").Contains(new Item("Item3")));
            Assert.IsTrue(dbFacade.GetItems("user1").Contains(new Item("Item4")));
            Assert.IsTrue(dbFacade.GetItems("user1").Contains(new Item("Item5")));

            //#Caching.4
            dbFacade.AddItems(new Item[] { new Item("Item1") }, "user3");
            Assert.AreEqual(dbFacade.GetItems("user3").Count, 1);
            Assert.IsTrue(dbFacade.GetItems("user3").Contains(new Item("Item1")));
        }
        [TestMethod]
        public void TestDeleteItems()
        {
            CachingDBFacade dbFacade = GetTestDBFacade();

            //#Caching.5
            dbFacade.DeleteItems(new Item[] { new Item("Item2"), new Item("Item3") }, "user1");
            Assert.AreEqual(dbFacade.GetItems("user1").Count, 1);
            Assert.IsTrue(dbFacade.GetItems("user1").Contains(new Item("Item1")));

            //#Caching.6
            dbFacade.AddItems(new Item[] { new Item("Item1") }, "user3");
            Assert.AreEqual(dbFacade.GetItems("user3").Count, 1);
            Assert.IsTrue(dbFacade.GetItems("user3").Contains(new Item("Item1")));
            dbFacade.DeleteItems(new Item[] { new Item("Item1") }, "user3");
            Assert.AreEqual(dbFacade.GetItems("user3").Count, 0);
            dbFacade.AddItems(new Item[] { new Item("Item1") }, "user3");
            Assert.AreEqual(dbFacade.GetItems("user3").Count, 1);
            Assert.IsTrue(dbFacade.GetItems("user3").Contains(new Item("Item1")));
        }
        [TestMethod]
        public void TestUpdateItems()
        {
            CachingDBFacade dbFacade = GetTestDBFacade();

            //#Caching.7
            dbFacade.UpdateItems(new Item[] {
                new Item("Name1", "Item1", TestType.Type, new Dictionary<string, string>()),
                new Item("Name2", "Item2", TestType.Type, new Dictionary<string, string>()) },
                "user1");
            Assert.AreEqual(dbFacade.GetItems("user1").Count, 3);
            foreach (Item item in dbFacade.GetItems("user1"))
            {
                switch (item.ProductNumber)
                {
                    case "Item1":
                        if (item.Name != "Name1")
                        {
                            Assert.Fail("Item1 not updated");
                        }
                        break;
                    case "Item2":
                        if (item.Name != "Name2")
                        {
                            Assert.Fail("Item2 not updated");
                        }
                        break;
                    case "Item3":
                        if (item.Name != null)
                        {
                            Assert.Fail("Item3 was updated");
                        }
                        break;
                }
            }
        }
        [TestMethod]
        public void TestHasItem()
        {
            CachingDBFacade dbFacade = GetTestDBFacade();

            //#Caching.8
            Assert.IsTrue(dbFacade.HasItem(new Item("Item3"), "user1"));

            //#Caching.9
            Assert.IsFalse(dbFacade.HasItem(new Item("Item3"), "user3"));
        }
        [TestMethod]
        public void TestItemType()
        {
            CachingDBFacade dbFacade = GetTestDBFacade();

            //#Caching.10
            Assert.AreEqual(dbFacade.GetItemType("empty").Name, "empty");

            //#Caching.11
            Assert.IsNull(dbFacade.GetItemType("not existing"));
        }

        private CachingDBFacade GetTestDBFacade()
        {
            return new CachingDBFacade(new TestClasses.TestDBFacade(new Dictionary<string, IList<Item>>()
            {
                {"user1", new List<Item>() {
                    new Item("Item1"), new Item("Item2"), new Item("Item3")
                } },
                {"user2", new List<Item>() {
                    new Item("Item1")
                } }
            }));
        }
    }
}
