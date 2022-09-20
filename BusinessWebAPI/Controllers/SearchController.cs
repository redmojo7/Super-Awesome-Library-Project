using API_Classes;
using BusinessWebAPI.App_Start;
using DatabaseServer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace BusinessWebAPI.Controllers
{
    [Route("api/Searching")]
    public class SearchController : ApiController
    {
        private readonly BusinessWebService businessWebService;

        public SearchController()
        {
            businessWebService = new BusinessWebService();
        }

        [HttpPost]
        public IHttpActionResult GetValuesForSearch(SearchData searchData)
        {
            Student student = businessWebService.GetValuesForSearch(searchData.searchStr);
            return Ok(student);
        }
    }
}