using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sem3FinalProject_Code.DBFacade
{
    public class ItemAlreadyPresentException : Exception
    {
        public ItemAlreadyPresentException(string msg) : base(msg) { }
    }
}