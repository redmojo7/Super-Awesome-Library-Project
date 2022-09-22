using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
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
                return File(bitmapdata, "image/png");
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult UploadFiles(List<IFormFile> files, int id)
        {
            //https://stackoverflow.com/questions/51021182/httppostedfilebase-in-asp-net-core-2-0
            string result = "Error";
            long size = files.Sum(f => f.Length);
            if (size == 0)
            {
                return BadRequest();
            }
            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(stream);
                        // process uploaded files
                        // Don't rely on or trust the FileName property without validation.

                        //Read the uploaded File as Byte Array from FileUpload control.
                        //Stream fs = file.InputStream;
                        BinaryReader br = new BinaryReader(stream);
                        RestClient client = new RestClient(URL);
                        RestRequest request = new RestRequest("api/Students/avarta/{id}", Method.Post);
                        request.AddUrlSegment("id", id);
                        request.AddFile("avarta", br.ReadBytes((Int32)stream.Length), formFile.FileName);
                        request.AddHeader("Content-Type", "multipart/form-data");
                        RestResponse response = client.Execute(request);
                        if (response != null)
                        {
                            if (response.IsSuccessful)
                            {
                                result = "success";
                            }
                        }
                    }
                }
            }
            return Ok(new { count = files.Count, size, filePath });
        }
    }
}
