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
        private StudentServerInterface foob;

        public SearchController()
        {
            businessWebService = new BusinessWebService();
            foob = new BusinessWebService().foob;
        }

        [HttpPost]
        public IHttpActionResult GetValuesForSearch(SearchData searchData)
        {
            //Thread.Sleep(1000);
            DataIntermed student = null;
            int numEntry = foob.GetNumEntries();
            for (int index = 1; index <= numEntry; index++)
            {
                string firstName, lastName;
                int balance;
                uint acctNo, pin;
                Bitmap profileBitmap;
                foob.GetValuesForEntry(index, out acctNo, out pin, out balance, out firstName, out lastName, out profileBitmap);
                if (firstName.ToLower().Contains(searchData.searchStr.ToLower()))
                {
                    student = new DataIntermed(pin, acctNo, firstName, lastName, balance, null);
                    break;
                }
            }
            return Ok(student);
        }
    }
}