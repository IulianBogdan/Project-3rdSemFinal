using Sem3FinalProject_Code.DBFacade;
using Sem3FinalProject_Code.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sem3FinalProject_Code.Controllers
{
    [Authorize]
    [RoutePrefix("api/Items")]
    public class ItemsController : ApiController
    {
        [HttpGet]
        [Route("Get")]
        public IHttpActionResult GetItems()
        {
            return Ok(ApplicationState.DBFacade.GetItems(User.Identity.Name).Select((item) => new ItemBindingModel(item)));
        }

        [HttpPost]
        [Route("Add")]
        public IHttpActionResult AddItems([FromBody] ItemBindingModel[] items)
        {
            Item[] actualItems = null;

            try
            {
                actualItems = ConvertItems(items);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }

            try
            {
                ApplicationState.DBFacade.AddItems(actualItems.ToArray(), User.Identity.Name);
            }
            catch (Exception ex)
            {
                if (ex is ItemAlreadyPresentException || ex is DuplicatedItemException)
                {
                    return BadRequest(ex.Message);
                }
                return InternalServerError();
            }

            return Ok("Operation successfull");
        }

        [HttpPut]
        [Route("Update")]
        public IHttpActionResult UpdateItems(ItemBindingModel[] items)
        {
            Item[] actualItems = null;

            try
            {
                actualItems = ConvertItems(items);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }

            try
            {
                ApplicationState.DBFacade.UpdateItems(actualItems, User.Identity.Name);
            }
            catch (Exception ex)
            {
                if (ex is DuplicatedItemException || ex is ItemNotPresentException || ex is ItemTypeChangedException)
                {
                    return BadRequest(ex.Message);
                }
                return InternalServerError();
            }

            return Ok("Operation successfull");
        }

        [HttpPut]
        [Route("Delete")]
        public IHttpActionResult DeleteItems(DeleteItemBindingModel[] items)
        {
            Item[] actualItems = items.Select((item) => new Item(item.ProductNumber)).ToArray();

            try
            {
                ApplicationState.DBFacade.DeleteItems(actualItems, User.Identity.Name);
            }
            catch (Exception ex)
            {
                if (ex is DuplicatedItemException || ex is ItemNotPresentException)
                {
                    return BadRequest(ex.Message);
                }
                return InternalServerError();
            }

            return Ok("Operation successfull");
        }
        /*
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateAccount(string email, string password, string producer)
        {
            return Ok("banana");
        }
        */
    
        private Item[] ConvertItems(ItemBindingModel[] items)
        {
            IList<Item> actualItems = new List<Item>(items.Length);
            for (int i = 0; i < items.Length; i++)
            {
                ItemType type = ApplicationState.DBFacade.GetItemType(items[i].ItemTypeName);
                if (type == null)
                {
                    throw new BadRequestException("Item type of item at position " + i + " is non existant");
                }
                try
                {
                    actualItems.Add(new Item(items[i].Name, items[i].ProductNumber, type, items[i].Properties));
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentException || ex is FormatException)
                    {
                        throw new BadRequestException("Error in Item at position " + i + ": " + ex.Message);
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            return actualItems.ToArray();
        }
    }

    internal class BadRequestException : Exception
    {
        internal BadRequestException(string msg) : base(msg) { }
    }
}
