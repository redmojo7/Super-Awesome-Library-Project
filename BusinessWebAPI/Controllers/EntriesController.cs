using BusinessWebAPI.App_Start;
using DatabaseServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BusinessWebAPI.Controllers
{

    [Route("api/entries")]
    public class EntriesController : ApiController
    {

        private readonly BusinessWebService businessWebService;
        private StudentServerInterface foob;

        public EntriesController()
        {
            businessWebService = new BusinessWebService();
            foob = new BusinessWebService().foob;
        }

        [HttpGet]
        public IHttpActionResult GetNumEntries()
        {
            return Ok(foob.GetNumEntries());
        }
    }
}
