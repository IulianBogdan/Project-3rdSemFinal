using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sem3FinalProject_Code.Models
{
    public class Item
    {
        private IDictionary<string, Property> _properties = new Dictionary<string, Property>();
        public IList<Property> Properties
        {
            get
            {
                CheckValid();
                IList<Property> res = Type.DefaultProperties;
                for (int i = 0; i < res.Count; i++)
                {
                    if (_properties.ContainsKey(res[i].Name))
                    {
                        res[i] = _properties[res[i].Name];
                    }
                }
                return res;
            }
        }
        public ItemType Type { get; private set; }
        public string Name { get; private set; }
        public string ProductNumber { get; private set; }

        public Item(string name, string productNumber, ItemType itemType, IDictionary<string, string> properties)
        {
            if (name == null || productNumber == null || itemType == null)
            {
                throw new ArgumentNullException();
            }
            Name = name;
            ProductNumber = productNumber;
            Type = itemType;
            foreach (var property in properties)
            {
                Property defaultProperty = Type.GetDefaultProperty(property.Key);
                if (defaultProperty == null)
                {
                    throw new ArgumentException("One or more of the properties can't be in this type of item");
                }
                this._properties.Add(property.Key, new Property(property.Value, property.Key, defaultProperty.Type));
            }
        }

        public Item(string productNumber)
        {
            ProductNumber = productNumber;
        }

        /// <summary>
        /// Returns the property with the provided name if it exist, or null if it doesn't. <br/>
        /// If the property was never set it will return the default value for that property in the item type of this.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the item was initialized without a type</exception>
        public Property GetProperty(string name)
        {
            CheckValid();
            try
            {
                return _properties[name];
            }
            catch (KeyNotFoundException)
            {
                return Type.GetDefaultProperty(name);
            }
        }

        /// <summary>
        /// If the item type of this accept a property with the provided name, modifies the corresponding property value in this item and returns true, otherwise it just returns false.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the item was initialized without a type</exception>
        public bool SetProperty(string name, string value)
        {
            CheckValid();
            Property defaultProp = Type.GetDefaultProperty(name);
            if (defaultProp == null)
            {
                return false;
            }
            _properties[name] = new Property(value, name, defaultProp.Type);
            return true;
        }

        private void CheckValid()
        {
            if (Type == null)
            {
                throw new InvalidOperationException("The item does not have any type.");
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Item && ((Item) obj).ProductNumber == ProductNumber;
        }

        public override int GetHashCode()
        {
            return ProductNumber.GetHashCode();
        }
    }
}