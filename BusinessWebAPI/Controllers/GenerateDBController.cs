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

    [Route("api/GenerateDB")]
    public class GenerateDBController : ApiController
    {

        private readonly BusinessWebService businessWebService;

        public GenerateDBController()
        {
            businessWebService = new BusinessWebService();
        }

        [HttpGet]
        public IHttpActionResult GenerateDB()
        {
            return Ok(businessWebService.GenerateDB());
        }
    }
}
