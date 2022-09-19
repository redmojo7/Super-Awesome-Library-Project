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

    [Route("api/GenerateData")]
    public class GenerateDataController : ApiController
    {

        private readonly BusinessWebService businessWebService;

        public GenerateDataController()
        {
            businessWebService = new BusinessWebService();
        }

        [HttpGet]
        public IHttpActionResult GenerateData()
        {
            return Ok(businessWebService.GenerateData());
        }
    }
}
