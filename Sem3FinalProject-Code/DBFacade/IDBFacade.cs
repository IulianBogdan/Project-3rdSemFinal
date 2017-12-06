using Sem3FinalProject_Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sem3FinalProject_Code.DBFacade
{
    public interface IDBFacade
    {
        IList<Item> GetItems(string producerEmail); 
        void AddItems(Item[] items, string producerEmail);
        void UpdateItems(Item[] items, string producerEmail);
        void DeleteItems(Item[] items, string producerEmail);
        bool HasItem(Item item, string producerEmail);
        ItemType GetItemType(string typeName);
    }
}
