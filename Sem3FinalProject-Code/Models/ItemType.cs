using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sem3FinalProject_Code.Models
{
    public class ItemType
    {
        private IDictionary<string, Property> _defaultProperties;

        public IList<Property> DefaultProperties
        {
            get
            {
                return new List<Property>(_defaultProperties.Values);
            }
        }
        public string Name { get; private set; }
        public ItemType(string name, IDictionary<string, Property> defaultProperties)
        {
            Name = name;
            this._defaultProperties = new Dictionary<string, Property>(defaultProperties);
        }

        public Property GetDefaultProperty(string name)
        {
            try
            {
                return _defaultProperties[name];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }
}