using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sem3FinalProject_Code.Models;

namespace Sem3FinalProject_Code.DBFacade
{
    public class CheckingDBFacade : IDBFacade
    {
        private IDBFacade component;

        public CheckingDBFacade(IDBFacade component)
        {
            this.component = component;
        }

        public void AddItems(Item[] items, string producerEmail)
        {
            CheckAllDifferent(items);
            CheckAllNotPresent(items, producerEmail);
            component.AddItems(items, producerEmail);
        }

        public void DeleteItems(Item[] items, string producerEmail)
        {
            CheckAllDifferent(items);
            CheckAllPresent(items, producerEmail);
            component.DeleteItems(items, producerEmail);
        }

        public void UpdateItems(Item[] items, string producerEmail)
        {
            CheckAllDifferent(items);
            CheckAllPresent(items, producerEmail);
            component.UpdateItems(items, producerEmail);
        }

        public IList<Item> GetItems(string producerEmail)
        {
            return component.GetItems(producerEmail);
        }

        public ItemType GetItemType(string typeName)
        {
            return component.GetItemType(typeName);
        }

        private void CheckAllNotPresent(Item[] items, string producerEmail)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (component.HasItem(items[i], producerEmail))
                {
                    throw new ItemAlreadyPresentException("An item with the same identifier as the one at position " + i + " is already present");
                }
            }
        }
        private void CheckAllPresent(Item[] items, string producerEmail)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (!component.HasItem(items[i], producerEmail))
                {
                    throw new ItemNotPresentException("There is not item with the same identifier as the one at position " + i);
                }
            }
        }

        private void CheckAllDifferent(Item[] items)
        {
            IDictionary<string, int> firstOccurrences = new Dictionary<string, int>();
            for (int i = 0; i < items.Length; i++)
            {
                if (firstOccurrences.ContainsKey(items[i].ProductNumber))
                {
                    throw new DuplicatedItemException("Item at position " + i + " duplicated. " +
                        "First occurence " + firstOccurrences[items[i].ProductNumber]);
                }
                firstOccurrences.Add(items[i].ProductNumber, i);
            }
        }

        public bool HasItem(Item item, string producerEmail)
        {
            return component.HasItem(item, producerEmail);
        }
    }
}