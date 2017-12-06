using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sem3FinalProject_Code.Models
{
    public class Item
    {
        private IDictionary<string, Property> properties = new Dictionary<string, Property>();
        private ItemType type;
        public string Name { get; set; }
        public string ProductNumber { get; set; }
        public string ItemTypeName { get; set; }

        //Item(name: string, productNumber: string, itemTypeName: string)

        /// <summary>
        /// Returns the property with the provided name if it exist, or null if it doesn't. <br/>
        /// If the property was never set it will return the default value for that property in the item type of this.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the item was not validated yet</exception>
        public Property GetProperty(string name)
        {
            CheckValid();
            try
            {
                return properties[name];
            }
            catch (KeyNotFoundException)
            {
                return type.GetDefaultProperty(name);
            }
        }

        /// <summary>
        /// If the item type of this accept a property with the provided name, modifies the corresponding property value in this item and returns true, otherwise it just returns false.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the item was not validated yet</exception>
        public bool SetProperty(string name, string value)
        {
            CheckValid();
            Property defaultProp = type.GetDefaultProperty(name);
            if (defaultProp == null)
            {
                return false;
            }
            properties[name] = new Property(value, name, defaultProp.Type);
            return true;
        }

        /// <summary>
        /// Checks if the item type for this item actually exists, and returns true. Otherwise returns false.
        /// To be called before performing any other operation
        /// </summary>
        public bool Validate()
        {
            throw new NotImplementedException();
        }

        private void CheckValid()
        {
            if (type == null)
            {
                throw new InvalidOperationException("The item was not validated yet.");
            }
        }
    }
}