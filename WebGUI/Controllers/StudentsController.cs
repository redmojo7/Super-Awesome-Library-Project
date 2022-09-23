using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using WebGUI.Models;
using static System.Net.Mime.MediaTypeNames;

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
            if (id == 0)
            {
                return BadRequest("id cannot be null or 0");
            }
            RestClient restClient = new RestClient(URL);
            RestRequest restRequest = new RestRequest("api/student/Get", Method.Get);
            restRequest.AddParameter("id", id);
            RestResponse restResponse = restClient.Execute(restRequest);
            if (restResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(string.Format("Student with id = {0} was not found", id));
            }
            return Ok(restResponse.Content);
        }

        /*
        [HttpGet]
        public IActionResult Search(string searchText)
        {
            RestClient restClient = new RestClient(URL);
            RestRequest restRequest = new RestRequest("api/search", Method.Get);
            restRequest.AddParameter("searchText", searchText);
            RestResponse restResponse = restClient.Execute(restRequest);
            return Ok(restResponse.Content);
        }
        */

        [HttpPost]
        public IActionResult Insert([FromBody] Student student)
        {
            if (student == null)
            {
                return BadRequest();
            }
            RestClient restClient = new RestClient(URL);
            RestRequest restRequest = new RestRequest("api/student/insert", Method.Post);
            restRequest.AddBody(student);
            RestResponse restResponse = restClient.Execute(restRequest);

            Student returnStudent = null;
            if (restResponse.IsSuccessful)
            {
                JsonConvert.DeserializeObject<Student>(restResponse.Content);
            }
            else if (restResponse.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                return Conflict(string.Format("Student with id = {0} already exist.", student.Id));
            }
            return Ok(returnStudent);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            RestClient client = new RestClient(URL);
            // send http request to delete
            RestRequest restRequest = new RestRequest("api/student/{id}", Method.Delete);
            restRequest.AddUrlSegment("id", id);
            RestResponse restResponse = client.Execute(restRequest);
            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("Delete Successful!");
            }
            else if (restResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(string.Format("Student with id = {0} was not found", id));
            }
            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromBody] Student student)
        {
            if (student == null)
            {
                return BadRequest();
            }
            RestClient client = new RestClient(URL);
            // send http request to update
            RestRequest restRequest = new RestRequest("api/student/update", Method.Put);
            restRequest.AddBody(student);
            RestResponse restResponse = client.Execute(restRequest);
            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("Update Successful!");
            }
            else if (restResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(string.Format("Student with id = {0} was not found", student.Id));
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult GenerateDB()
        {
            RestClient client = new RestClient(URL);
            // send http request to generate db
            RestRequest restRequest = new RestRequest("api/GenerateDB", Method.Get);
            RestResponse restResponse = client.Execute(restRequest);
            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("GenerateDB Successful!");
            }
            return Ok();
        }

        
        [HttpGet]
        public IActionResult Profile(string path)
        {
            if (path != null)
            {
                // send http request to search
                RestClient client = new RestClient(URL);
                RestRequest restRequest = new RestRequest("api/profile", Method.Get);
                restRequest.AddParameter("path", path);
                byte[] bitmapdata = client.DownloadData(restRequest);
                //return File(bitmapdata, "image/png");
                string base64ImageRepresentation = Convert.ToBase64String(bitmapdata);
                return Ok(base64ImageRepresentation);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult UploadFiles(int id)
        {
            try
            {
                long size = Request.Form.Files.Sum(f => f.Length);
                if (size == 0 || id == 0)
                {
                    return BadRequest("avarta or id empty");
                }
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0 || id == 0)
                {
                    //Read the uploaded File as Byte Array from FileUpload control.
                    Stream fs = file.OpenReadStream();
                    BinaryReader br = new BinaryReader(fs);

                    RestClient client = new RestClient(URL);
                    RestRequest request = new RestRequest("api/Students/avarta/{id}", Method.Post);
                    request.AddUrlSegment("id", id);
                    request.AddFile("avarta", br.ReadBytes((Int32)fs.Length), file.FileName);
                    request.AddHeader("Content-Type", "multipart/form-data");
                    RestResponse response = client.Execute(request);
                    if (response != null)
                    {
                        if (!response.IsSuccessful)
                        {
                            return StatusCode(500, $"Internal server error.");
                        }
                    }
                    return Ok();
                }
                else
                {
                    return BadRequest("file is empty");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
