using API_Classes;
using BusinessWebAPI.App_Start;
using DatabaseServer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;

namespace BusinessWebAPI.Controllers
{
    //[Route("api/student")]
    public class StudentController : ApiController
    {
        private readonly BusinessWebService businessWebService;


        public StudentController()
        {
            businessWebService = new BusinessWebService();
        }


        [HttpPost]
        public IHttpActionResult Insert(DataIntermed student)
        {
            try
            {
                businessWebService.Insert(student);
            }
            catch (FaultException<ArgumentOutOfRangeException> oe)
            {
                Console.WriteLine(oe.Message);
                var message = string.Format("Parameter with index = {0} is out of range", 0);
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult Update(DataIntermed student)
        {
            try
            {
                businessWebService.Update(student);
            }
            catch (FaultException<ArgumentOutOfRangeException> oe)
            {
                Console.WriteLine(oe.Message);
                var message = string.Format("Parameter with index = {0} is out of range", 0);
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
            return Ok();
        }


        [HttpDelete]
        public IHttpActionResult Delete(uint AcctNo)
        {
            try
            {
                businessWebService.Delete(AcctNo);
            }
            catch (FaultException<ArgumentOutOfRangeException> oe)
            {
                Console.WriteLine(oe.Message);
                var message = string.Format("Parameter with index = {0} is out of range", 0);
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
            return Ok();
        }
    }
}
