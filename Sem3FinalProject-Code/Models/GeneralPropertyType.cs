using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sem3FinalProject_Code.Models
{
    public class GeneralPropertyType : IPropertyType
    {
        private string name;
        private Predicate<string> validator;
        public GeneralPropertyType(string name, Predicate<string> validator)
        {
            this.name = name;
            this.validator = validator;
        }

        public string GetName()
        {
            return name;
        }

        public bool Validate(string value)
        {
            return validator.Invoke(value);
        }
    }
}