using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sem3FinalProject_Code.Models
{
    public class PropertyTypeFactory : IPropertyTypeFactory
    {
        IDictionary<string, IPropertyType> types = new Dictionary<string, IPropertyType>();

        public PropertyTypeFactory()
        {
            AddPropertyType("bool", (val) => {
                return val.ToLower() == "true" || val.ToLower() == "false";
            });
            AddPropertyType("int", (val) =>
            {
                return Int32.TryParse(val, out int i);
            });
            AddPropertyType("long", (val) =>
            {
                return Int64.TryParse(val, out long i);
            });
            AddPropertyType("double", (val) =>
            {
                return Double.TryParse(val, out double i);
            });
            AddPropertyType("resolution", (val) =>
            {
                string[] vals = val.ToLower().Split('x');
                return vals.Length == 2 && Int32.TryParse(vals[0], out int val1) && Int32.TryParse(vals[1], out int val2) && val1 > 0 && val2 > 0;
            });
            AddPropertyType("string", (val) =>
            {
                return true;
            });
        }

        public IPropertyType GetPropertyType(string propertyName)
        {
            try
            {
                return types[propertyName.ToLower()];
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException("There is no property type with name \"" + propertyName + "\"");
            }
        }

        private void AddPropertyType(string name, Predicate<string> validator)
        {
            types.Add(name, new GeneralPropertyType(name, validator));
        }
    }
}
