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
    
    public class SearchController : ApiController
    {
        private readonly BusinessWebService businessWebService;

        public SearchController()
        {
            businessWebService = new BusinessWebService();
        }

        [Route("api/Searching")]
        [HttpPost]
        public IHttpActionResult GetValuesForSearch(SearchData searchData)
        {
            Student student = null;
            try
            {
                student = businessWebService.GetValuesForSearch(searchData.searchStr);
            }
            catch (Exception e)
            { 
                return NotFound();
            }
            return Ok(student);
        }

        [Route("api/search")]
        [HttpGet]
        public IHttpActionResult GetValuesForSearch(string searchText)
        {
            Student student = null;
            try
            {
                student = businessWebService.GetValuesForSearch(searchText);
            }
            catch (Exception e)
            {
                return NotFound();
            }
            return Ok(student);
        }
    }
}