using Sem3FinalProject_Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sem3FinalProject_Code.Controllers
{
    [RoutePrefix("api/Items")]
    public class MainController : ApiController
    {
        [HttpGet]
        [Route("Get")]
        IHttpActionResult GetItems()
        {
            return Ok("banana");
        }

        [HttpPost]
        [Route("Add")]
        IHttpActionResult AddItems(Item[] items)
        {
            return Ok("banana");
        }

        [HttpPut]
        [Route("Update")]
        IHttpActionResult UpdateItems(Item[] items)
        {
            return Ok("banana");
        }

        [HttpDelete]
        [Route("Delete")]
        IHttpActionResult DeleteItems(Item[] items)
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
