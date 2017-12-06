using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sem3FinalProject_Code.DBFacade
{
    public class ItemNotValidException : Exception
    {
        public ItemNotValidException(string msg) : base(msg) { }
    }
}