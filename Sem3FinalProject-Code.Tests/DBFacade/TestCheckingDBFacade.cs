using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sem3FinalProject_Code.DBFacade;
using Sem3FinalProject_Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sem3FinalProject_Code.Tests.DBFacade
{
    [TestClass]
    public class TestCheckingDBFacade
    {
        [TestMethod]
        public void TestAddItems()
        {
            CheckingDBFacade facade = GetTestDBFacade();

            //#checking.1
            try
            {
                facade.AddItems(new Item[] { new Item("Item1"), new Item("Item2"), new Item("Item1")}, "user3");
                Assert.Fail("Should have thrown DuplicatedItemException");
            }
            catch (Exception e)
            {
                if (!(e is DuplicatedItemException))
                {
                    Assert.Fail("Wrong exception. " + e.ToString());
                }
            }

            //#checking.2
            try
            {
                facade.AddItems(new Item[] { new Item("Item4"), new Item("Item2") }, "user1");
                Assert.Fail("Should have thrown ItemAlreadyPresentException");
            }
            catch (Exception e)
            {
                if (!(e is ItemAlreadyPresentException))
                {
                    Assert.Fail("Wrong exception. " + e.ToString());
                }
            }
        }

        [TestMethod]
        public void TestUpdateItems()
        {
            CheckingDBFacade facade = GetTestDBFacade();

            //#checking.3
            try
            {
                facade.UpdateItems(new Item[] { new Item("Item1"), new Item("Item2"), new Item("Item1") }, "user1");
                Assert.Fail("Should have thrown DuplicatedItemException");
            }
            catch (Exception e)
            {
                if (!(e is DuplicatedItemException))
                {
                    Assert.Fail("Wrong exception. " + e.ToString());
                }
            }

            //#checking.4
            try
            {
                facade.UpdateItems(new Item[] { new Item("Item2"), new Item("Item4") }, "user1");
                Assert.Fail("Should have thrown ItemNotPresentException");
            }
            catch (Exception e)
            {
                if (!(e is ItemNotPresentException))
                {
                    Assert.Fail("Wrong exception. " + e.ToString());
                }
            }
        }

        [TestMethod]
        public void TestDeleteItems()
        {
            CheckingDBFacade facade = GetTestDBFacade();

            //#checking.5
            try
            {
                facade.DeleteItems(new Item[] { new Item("Item1"), new Item("Item2"), new Item("Item1") }, "user1");
                Assert.Fail("Should have thrown DuplicatedItemException");
            }
            catch (Exception e)
            {
                if (!(e is DuplicatedItemException))
                {
                    Assert.Fail("Wrong exception. " + e.ToString());
                }
            }

            //#checking.6
            try
            {
                facade.DeleteItems(new Item[] { new Item("Item2"), new Item("Item4") }, "user1");
                Assert.Fail("Should have thrown ItemNotPresentException");
            }
            catch (Exception e)
            {
                if (!(e is ItemNotPresentException))
                {
                    Assert.Fail("Wrong exception. " + e.ToString());
                }
            }
        }

        private CheckingDBFacade GetTestDBFacade()
        {
            return new CheckingDBFacade(new TestClasses.TestDBFacade(new Dictionary<string, IList<Item>>()
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
