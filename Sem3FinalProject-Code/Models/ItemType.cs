using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sem3FinalProject_Code.Models
{
    public class ItemType
    {
        private IDictionary<string, Property> defaultProperties;
        public string Name { get; private set; }
        public ItemType(string name, IDictionary<string, Property> defaultProperties)
        {
            Name = name;
            this.defaultProperties = new Dictionary<string, Property>(defaultProperties);
        }

        public Property GetDefaultProperty(string name)
        {
            try
            {
                return defaultProperties[name];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }
}