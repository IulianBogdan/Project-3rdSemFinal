using Sem3FinalProject_Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sem3FinalProject_Code.DBFacade
{
    public class CachingDBFacade : IDBFacade
    {
        private object itemDictionaryLock = new object();
        private object itemTypeDictionaryLock = new object();
        private IDictionary<string, IDictionary<string, Item>> cachedItems = new Dictionary<string, IDictionary<string, Item>>();
        private IDictionary<string, ItemType> cachedItemTypes = new Dictionary<string, ItemType>();
        private IDBFacade component;

        public CachingDBFacade(IDBFacade component)
        {
            this.component = component;
        }
        public void AddProducer(string producerEmail, string producerName)
        {
            component.AddProducer(producerEmail, producerName);
        }

        public void AddItems(Item[] items, string producerEmail)
        {
            component.AddItems(items, producerEmail);
            lock (itemDictionaryLock)
            {
                FillCacheIfNeeded(producerEmail);
                foreach (Item item in items)
                {
                    cachedItems[producerEmail].Add(item.ProductNumber, item);
                }
            }
        }

        public void DeleteItems(Item[] items, string producerEmail)
        {
            component.DeleteItems(items, producerEmail);
            lock (itemDictionaryLock)
            {
                FillCacheIfNeeded(producerEmail);
                foreach (Item item in items)
                {
                    cachedItems[producerEmail].Remove(item.ProductNumber);
                }
            }
        }

        public void UpdateItems(Item[] items, string producerEmail)
        {
            component.UpdateItems(items, producerEmail);
            lock (itemDictionaryLock)
            {
                FillCacheIfNeeded(producerEmail);
                foreach (Item item in items)
                {
                    cachedItems[producerEmail][item.ProductNumber] = item;
                }
            }
        }

        public IList<Item> GetItems(string producerEmail)
        {
            lock (itemDictionaryLock)
            {
                FillCacheIfNeeded(producerEmail);
                return cachedItems[producerEmail].Values.ToList();
            }
        }

        public ItemType GetItemType(string typeName)
        {
            lock (itemTypeDictionaryLock)
            {
                if (!cachedItemTypes.ContainsKey(typeName))
                {
                    ItemType type = component.GetItemType(typeName);
                    if (type == null)
                    {
                        cachedItemTypes.Add(typeName, null);
                    }
                    else
                    {
                        cachedItemTypes.Add(type.Name, type);
                    }
                }
                return cachedItemTypes[typeName];
            }
        }

        public bool HasItem(Item item, string producerEmail)
        {
            lock (itemDictionaryLock)
            {
                FillCacheIfNeeded(producerEmail);
                return cachedItems[producerEmail].ContainsKey(item.ProductNumber);
            }
        }

        //fills cache if no other method for the same producer has been called before
        private void FillCacheIfNeeded(string producerEmail)
        {
            if (!cachedItems.ContainsKey(producerEmail))
            {
                //TODO are we sure?
                cachedItems.Add(producerEmail, component.GetItems(producerEmail).ToDictionary((item) => item.ProductNumber));
            }
        }
    }
}