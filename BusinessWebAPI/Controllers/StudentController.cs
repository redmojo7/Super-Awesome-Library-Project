using API_Classes;
using BusinessWebAPI.App_Start;
using DatabaseServer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

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
        public IHttpActionResult Insert(Student student)
        {
            try
            {
                string result = businessWebService.Insert(student);
                if (result != null) {
                    if (result == "Conflict")
                    {
                        return Conflict();
                    }
                    else if (result != "success")
                    {
                        return InternalServerError();
                    }
                }
            }
            catch (Exception oe)
            {
                Console.WriteLine(oe.Message);
                return InternalServerError();
            }
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult Update(Student student)
        {
            try
            {
                string result = businessWebService.Update(student);
                if (result != null)
                {
                    if (result == "NotFound")
                    {
                        return NotFound();
                    }
                    else if (result != "success")
                    {
                        return InternalServerError();
                    }
                }
            }
            catch (Exception oe)
            {
                Console.WriteLine(oe.Message);
                return InternalServerError();
            }
            return Ok();
        }


        [HttpDelete]
        public IHttpActionResult Delete(uint id)
        {
            try
            {
                string result = businessWebService.Delete(id);
                if (result != null)
                {
                    if (result == "NotFound")
                    {
                        return NotFound();
                    }
                    else if (result != "success")
                    {
                        return InternalServerError();
                    }
                }
            }
            catch (Exception oe)
            {
                Console.WriteLine(oe.Message);
                return InternalServerError();
            }
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Student student = null;
            try
            {
                student = businessWebService.Get(id);
            }
            catch (FaultException<ArgumentOutOfRangeException> oe)
            {
                Console.WriteLine(oe.Message);
                var message = string.Format("Student with index = {0} is out of range", 0);
                throw new HttpResponseException( 
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
            catch (Exception e)
            {
                var message = string.Format("Student with id = {0} was not found", id);
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
            return Ok(student);
        }

        [Route("api/Students/all")]
        [HttpGet]
        public IHttpActionResult All()
        {
            List<Student> students = null;
            try
            {
                students = businessWebService.All();
            }
            catch (Exception oe)
            {
                Console.WriteLine(oe.Message);
                return InternalServerError();
            }
            return Json(students);
        }

        [Route("api/Students/avarta/{id}")]
        [HttpPost]
        public IHttpActionResult avarta(int id)
        {
            HttpPostedFile file = HttpContext.Current.Request.Files[0];
            if (file.ContentLength > 0)
            {
                businessWebService.UploadAvarta(id, file);
            }
            else
            {
                BadRequest();
            }
            return Ok();
        }
    }
}
