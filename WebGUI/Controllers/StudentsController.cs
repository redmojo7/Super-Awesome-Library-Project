using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using WebGUI.Models;

namespace WebGUI.Controllers
{
    public class StudentsController : Controller
    {

        string URL = "http://localhost:54863/";
        public IActionResult Index()
        {
            ViewBag.Title = "Students";
            return View();
        }

        [HttpGet]
        public IActionResult Search(int id)
        {
            RestClient restClient = new RestClient(URL);
            RestRequest restRequest = new RestRequest("api/students/{id}", Method.Get);
            restRequest.AddUrlSegment("id", id);
            RestResponse restResponse = restClient.Execute(restRequest);
            return Ok(restResponse.Content);
        }
        [HttpPost]
        public IActionResult Insert([FromBody] Student student)
        {
            RestClient restClient = new RestClient(URL);
            RestRequest restRequest = new RestRequest("api/students", Method.Post);
            restRequest.AddJsonBody(JsonConvert.SerializeObject(student));
            RestResponse restResponse = restClient.Execute(restRequest);

            Student returnStudent = JsonConvert.DeserializeObject<Student>(restResponse.Content);
            if (returnStudent != null)
            {
                return Ok(returnStudent);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            return Ok("Delete");
        }

        [HttpPut]
        public IActionResult Update([FromBody] Student student)
        {
            return Ok("Update");
        }

        [HttpGet]
        public IActionResult GenerateDB()
        {
            return Ok("GenerateDB");
        }

        [HttpPost]
        public IActionResult UploadFiles([FromForm] IFormFile file)
        {
            try
            {
                // getting file original name
                string FileName = file.FileName;

                // combining GUID to create unique name before saving in wwwroot
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + FileName;

                // getting full path inside wwwroot/images
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", FileName);

                // copying file
                file.CopyTo(new FileStream(imagePath, FileMode.Create));

                return Ok("File Uploaded Successfully");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error"); ;
            }
        }

    }
}
