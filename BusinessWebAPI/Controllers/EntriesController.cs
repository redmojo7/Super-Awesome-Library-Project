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

        public EntriesController()
        {
            businessWebService = new BusinessWebService();
        }

        [HttpGet]
        public IHttpActionResult GetNumEntries()
        {
            return Ok(businessWebService.GetNumEntries());
        }
    }
}
