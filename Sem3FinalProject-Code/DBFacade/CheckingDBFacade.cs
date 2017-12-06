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
            CheckAllCanBeAdded(items, producerEmail);
            component.AddItems(items, producerEmail);
        }

        public void DeleteItems(Item[] items, string producerEmail)
        {
            CheckAllCanBeDeleted(items, producerEmail);
            component.DeleteItems(items, producerEmail);
        }

        public void UpdateItems(Item[] items, string producerEmail)
        {
            CheckAllCanBeUpdated(items, producerEmail);
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

        //TODO can be less copypasted
        private void CheckAllCanBeAdded(Item[] items, string producerEmail)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (!items[i].Validate())
                {
                    throw new ItemNotValidException("Item at position " + i + " is not valid.");
                }
                if (component.HasItem(items[i], producerEmail))
                {
                    throw new ItemAlreadyPresentException("An item with the same identifier as the one at position " + i + " is already present");
                }
            }
        }
        private void CheckAllCanBeUpdated(Item[] items, string producerEmail)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (!items[i].Validate())
                {
                    throw new ItemNotValidException("Item at position " + i + " is not valid.");
                }
                if (component.HasItem(items[i], producerEmail))
                {
                    throw new ItemNotPresentException("There is not item with the same identifier as the one at position " + i);
                }
            }
        }
        private void CheckAllCanBeDeleted(Item[] items, string producerEmail)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (component.HasItem(items[i], producerEmail))
                {
                    throw new ItemNotPresentException("There is not item with the same identifier as the one at position " + i);
                }
            }
        }

        public bool HasItem(Item item, string producerEmail)
        {
            return component.HasItem(item, producerEmail);
        }
    }
}