using Sem3FinalProject_Code.DBFacade;
using Sem3FinalProject_Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Sem3FinalProject_Code.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class MainController : ApiController
    {
        [HttpGet]
        [Route("GetItems")]
        IHttpActionResult GetItems()
        {
            return Ok(User.Identity.Name);
        }

        [HttpPost]
        [Route("Add")]
        IHttpActionResult AddItems(ItemBindingModel[] items)
        {
            IList<Item> actualItems = new List<Item>(items.Length);
            for (int i = 0; i < items.Length; i++)
            {
                ItemType type = ApplicationState.DBFacade.GetItemType(items[i].ItemTypeName);
                if (type == null)
                {
                    return BadRequest("Item type of item at position " + i + " is non existant");
                }
                try
                {
                    actualItems.Add(new Item(items[i].Name, items[i].ProductNumber, type, items[i].Properties));
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentException || ex is FormatException)
                    {
                        return BadRequest("Error in Item at position " + i + ": " + ex.Message);
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            try
            {
                ApplicationState.DBFacade.AddItems(actualItems.ToArray(), User.Identity.Name);
            }
            catch (Exception ex)
            {
                if (ex is ItemAlreadyPresentException || ex is ItemNotPresentException) { 
                    return BadRequest(ex.Message);
                }
                return InternalServerError();
            }
            return Ok("Operation successfull");
        }

        [HttpPut]
        [Route("Update")]
        IHttpActionResult UpdateItems(ItemBindingModel[] items)
        {
            return Ok("banana");
        }

        [HttpDelete]
        [Route("Delete")]
        IHttpActionResult DeleteItems(DeleteItemBindingModel[] items)
        {
            return Ok("banana");
        }
        /*
        [HttpPost]
        [Route("")]
        IHttpActionResult CreateAccount(string email, string password, string producer)
        {
            return Ok("banana");
        }
        */
    }
}
