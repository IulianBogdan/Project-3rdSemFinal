using Sem3FinalProject_Code.DBFacade;
using Sem3FinalProject_Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sem3FinalProject_Code
{
    public static class ApplicationState
    {
        public static IDBFacade DBFacade { get; private set; } = new CheckingDBFacade(new CachingDBFacade(new TestDBFacade()));
        public static IPropertyTypeFactory PropTypeFactory { get; private set; } = new PropertyTypeFactory();
    }
}