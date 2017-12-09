using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sem3FinalProject_Code.Models;
using Sem3FinalProject_Code.DBFacade;

namespace Sem3FinalProject_Code.Tests.TestClasses
{
    public class TestDBFacade : IDBFacade
    {
        private ItemType emptyType;
        private IDictionary<string, IList<Item>> data;

        public TestDBFacade() : this(new Dictionary<string, IList<Item>>())
        {
        }

        public TestDBFacade(IDictionary<string, IList<Item>> data)
        {
            Dictionary<string, Property> dictionary = new Dictionary<string, Property>();
            emptyType = new ItemType("empty", dictionary);
            IPropertyTypeFactory pf = new PropertyTypeFactory();
            this.data = data;
        }

        public void AddItems(Item[] items, string producerEmail)
        {
        }

        public void AddProducer(string producerEmail, string producerName)
        {
        }

        public void DeleteItems(Item[] items, string producerEmail)
        {
        }

        public IList<Item> GetItems(string producerEmail)
        {
            try
            {
                return data[producerEmail];
            }
            catch (KeyNotFoundException)
            {
                return new List<Item>();
            }
        }

        public ItemType GetItemType(string typeName)
        {
            if (typeName == "empty")
            {
                return emptyType;
            }
            return null;
        }

        public bool HasItem(Item item, string producerEmail)
        {
            try
            {
                return data[producerEmail].Contains(item);
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        public void UpdateItems(Item[] items, string producerEmail)
        {
        }

        
    }
}