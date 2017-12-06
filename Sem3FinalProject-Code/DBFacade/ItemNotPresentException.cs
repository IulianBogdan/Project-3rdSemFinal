using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sem3FinalProject_Code.DBFacade
{
    public class ItemNotPresentException : Exception
    {
        public ItemNotPresentException(string msg) : base(msg) { }
    }
}