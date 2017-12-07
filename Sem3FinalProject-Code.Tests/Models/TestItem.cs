using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sem3FinalProject_Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sem3FinalProject_Code.Tests.Models
{
    [TestClass]
    public class TestItem
    {
        [TestMethod]
        public void TestConstructor()
        {
            //#Item.1
            try
            {
                new Item("Name1", "Item1", TestClasses.TestType.Type, new Dictionary<string, string>() {
                { "screenSizeInches", "Banana" }
                });
                Assert.Fail("Banana is not an acceptable value for screen size inches");
            }
            catch (Exception e)
            {
                if (!(e is FormatException))
                {
                    Assert.Fail("Wrong exception thrown");
                }
            }

            //#Item.2
            try
            {
                new Item("Name1", "Item1", TestClasses.TestType.Type, new Dictionary<string, string>() {
                { "banana", "coconut" }
                });
                Assert.Fail("Banana is not a property of test type");
            }
            catch (Exception e)
            {
                if (!(e is ArgumentException))
                {
                    Assert.Fail("Wrong exception thrown");
                }
            }
        }

        [TestMethod]
        public void TestProperties()
        {
            Item item = new Item("Name1", "Item1", TestClasses.TestType.Type, new Dictionary<string, string>() {
                { "screenSizeInches", "3.5" }
                });

            //#Item.3
            Assert.AreEqual(item.GetProperty("screenSizeInches").Value, "3.5");

            //#Item.4
            Assert.IsNull(item.GetProperty("thisDoesNotExistForSure"));
            Assert.IsFalse(item.SetProperty("thisDoesNotExistForSure", "banana"));

            //#Item.5
            IList<Property> props = item.Properties;
            Assert.AreEqual(props.Count, 2);
            foreach (Property prop in props)
            {
                switch (prop.Name)
                {
                    case "screenSizeInches":
                        Assert.AreEqual("3.5", prop.Value);
                        break;
                    case "screenResolution":
                        Assert.AreEqual("123X567", prop.Value);
                        break;
                }
            }

            //#Item.6
            item.SetProperty("screenResolution", "234x345");
            Assert.AreEqual("234x345", item.GetProperty("screenResolution").Value);
        }
    }
}
