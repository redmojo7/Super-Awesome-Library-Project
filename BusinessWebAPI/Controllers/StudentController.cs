﻿using API_Classes;
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
        public IHttpActionResult Delete(uint Id)
        {
            try
            {
                string result = businessWebService.Delete(Id);
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
            catch (FaultException<ArgumentOutOfRangeException> oe)
            {
                Console.WriteLine(oe.Message);
                var message = string.Format("Parameter with index = {0} is out of range", 0);
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult Get(int Id)
        {
            Student student = null;
            try
            {
                student = businessWebService.Get(Id);
                /*
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
                */
            }
            catch (FaultException<ArgumentOutOfRangeException> oe)
            {
                Console.WriteLine(oe.Message);
                var message = string.Format("Parameter with index = {0} is out of range", 0);
                throw new HttpResponseException( 
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
            catch (Exception e)
            {
                var message = string.Format("Parameter with id = {0} was not found", Id);
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
            return Ok(student);
        }
    }
}