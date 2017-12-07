using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sem3FinalProject_Code.DBFacade
{
    public class DuplicatedItemException : Exception
    {
        public DuplicatedItemException(string msg) : base(msg) { }
    }
}