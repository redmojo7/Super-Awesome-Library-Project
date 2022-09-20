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

        public GetValuesController()
        {
            businessWebService = new BusinessWebService();
        }

        [HttpGet]
        public IHttpActionResult GetValuesForEntry(int index)
        {
            Student student = null;
            try
            {
                student = businessWebService.GetValuesForEntry(index);
            }
            catch (FaultException<ArgumentOutOfRangeException> oe)
            {
                Console.WriteLine(oe.Message);
                var message = string.Format("Parameter with index = {0} is out of range", index);
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
            return Ok(student);
        }

    }
}
