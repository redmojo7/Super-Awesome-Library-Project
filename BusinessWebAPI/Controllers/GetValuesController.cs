using API_Classes;
using BusinessWebAPI.App_Start;
using BusinessWebAPI.CustomException;
using DatabaseServer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace BusinessWebAPI.Controllers
{
    [Route("api/getValues")]
    public class GetValuesController : ApiController
    {
        private readonly BusinessWebService businessWebService;
        private StudentServerInterface foob;

        public GetValuesController()
        {
            businessWebService = new BusinessWebService();
            foob = new BusinessWebService().foob;
        }

        [HttpGet]
        public IHttpActionResult GetValuesForEntry(int index)
        {
            uint acctNo = 0, pin = 0;
            int balance = 0;
            string firstName = null, lastName = null;
            Bitmap profileBitmap = null;
            DataIntermed student = null;
            try
            {
                foob.GetValuesForEntry(index, out acctNo, out pin, out balance, out firstName, out lastName, out profileBitmap);
                student = new DataIntermed(pin, acctNo, firstName, lastName, balance, null);
            }
            catch (FaultException<ArgumentOutOfRangeException> oe)
            {
                Console.WriteLine(oe.Message);
                // This FaultException<T> exception results in a fault being sent to the client.
                //throw new FaultException<ArgumentOutOfRangeException>(new ArgumentOutOfRangeException("", oe.Message), "Parameter index is out of range.");
                //return BadRequest("Parameter index is out of range.");
                //throw new InvalidIndexException("Parameter index is out of range.");
                var message = string.Format("Parameter with index = {0} is out of range", index);
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
            return Ok(student);
        }

    }
}
