using Sem3FinalProject_Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sem3FinalProject_Code.Tests.TestClasses
{
    static class TestType
    {
        public static ItemType Type;
        static TestType() {
            IPropertyTypeFactory pf = new PropertyTypeFactory();
            IDictionary<string, Property> dictionary = new Dictionary<string, Property>() {
                { "screenSizeInches", new Property("5.5", "screenSizeInches", pf.GetPropertyType("double")) },
                { "screenResolution", new Property("123X567", "screenResolution", pf.GetPropertyType("Resolution"))}
            };
            Type = new ItemType("test", dictionary);
        }
    }
}
