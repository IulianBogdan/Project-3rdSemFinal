using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sem3FinalProject_Code.Models
{
    public interface IPropertyType
    {
        string GetName();
        bool Validate(string value);
    }
}
