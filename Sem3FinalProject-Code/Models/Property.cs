using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sem3FinalProject_Code.Models
{
    public class Property
    {
        public string Name { get; private set; }
        private string _value;
        public string Value {
            get {
                return _value;
            }
            private set {
                if (!Type.Validate(value))
                {
                    throw new FormatException("Value for property \"" + Name + "\" must be of type " + Type.GetName());
                }
                this._value = value;
            }
        }
        public IPropertyType Type { get; private set; }

        public Property(string value, string name, IPropertyType type)
        {
            Type = type;
            Name = name;
            Value = value;
        }
    }
}