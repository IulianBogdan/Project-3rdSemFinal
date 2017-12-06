using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sem3FinalProject_Code.Models;

namespace Sem3FinalProject_Code.DBFacade
{
    public class TestDBFacade : IDBFacade
    {
        private ItemType testType;

        public TestDBFacade()
        {
            Dictionary<string, Property> dictionary = new Dictionary<string, Property>();
            testType = new ItemType("test", dictionary);
            dictionary.Add("banana", new Property("banana_tree", "banana", ApplicationState.PropTypeFactory.GetPropertyType("string")));
            dictionary.Add("coconut", new Property("123X567", "coconut", ApplicationState.PropTypeFactory.GetPropertyType("Resolution")));
        }

        public void AddItems(Item[] items, string producerEmail)
        {
        }

        public void DeleteItems(Item[] items, string producerEmail)
        {
        }

        public IList<Item> GetItems(string producerEmail)
        {
            return new List<Item>();
        }

        public ItemType GetItemType(string typeName)
        {
            if (typeName == "test")
            {

            }
            return null;
        }

        public bool HasItem(Item item, string producerEmail)
        {
            throw new NotImplementedException();
        }

        public void UpdateItems(Item[] items, string producerEmail)
        {
            throw new NotImplementedException();
        }
    }
}