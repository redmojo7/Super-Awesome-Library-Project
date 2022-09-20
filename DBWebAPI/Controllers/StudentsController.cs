using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DBWebAPI.Models;
using DBWebAPI.service;

namespace DBWebAPI.Controllers
{
    public class StudentsController : ApiController
    {
        private studentdbEntities db = new studentdbEntities();
        private StudentService studentService;
        private const string resourcesPath = "Resources";

        public StudentsController()
        {
            studentService = new StudentService();
        }

        // GET: api/Students
        public IQueryable<Student> GetStudents()
        {
            return db.Students;
        }

        // GET: api/Students/5
        [ResponseType(typeof(Student))]
        public IHttpActionResult GetStudent(int id)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // PUT: api/Students/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStudent(int id, Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.Id)
            {
                return BadRequest();
            }

            db.Entry(student).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Students
        [ResponseType(typeof(Student))]
        public IHttpActionResult PostStudent(Student student)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            

            db.Students.Add(student);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (StudentExists(student.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [ResponseType(typeof(Student))]
        public IHttpActionResult DeleteStudent(int id)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            db.Students.Remove(student);
            db.SaveChanges();

            return Ok(student);
        }

        [Route("api/Students/GenerateDB")]
        [HttpGet]
        public IHttpActionResult GenerateDB()
        {
            try
            {
                db.Students.RemoveRange(db.Students);
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            List<Student> students = studentService.GenerateDB();
            
            foreach (Student stu in students)
            {
                // add Student to DB
                db.Students.Add(stu);
            }

            // SaveChanges to DB
            
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
               throw;
            }
            return Ok();
        }

        [Route("api/Students/profile")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetProfile(string path)
        {
            HttpResponseMessage response;
            if (path != null)
            {

                // Retrieve the profile image.
                string profileFullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, resourcesPath, path);
                Bitmap profileBitmap = new Bitmap(profileFullPath);

                Byte[] b;
                using (var stream = new MemoryStream())
                {
                    profileBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    b = stream.ToArray();
                }
                response = new HttpResponseMessage();
                response.Content = new ByteArrayContent(b);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return response;
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                return response;
            }
        }


        [Route("api/Students/search")]
        [HttpGet]
        public IHttpActionResult search(string searchText)
        {
            if (searchText != null)
            {
                foreach (Student s in db.Students)
                {
                    if (s.FirstName.ToLower().Contains(searchText.ToLower()) || s.LastName.ToLower().Contains(searchText.ToLower()))
                    {
                        return Ok(s);
                    }
                }
                return NotFound();
            }
            else
            {
                return BadRequest();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentExists(int id)
        {
            return db.Students.Count(e => e.Id == id) > 0;
        }
    }
}