using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using WebGUI.Models;

namespace WebGUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Home";

            RestClient restClient = new RestClient("http://localhost:54863/");
            RestRequest restRequest = new RestRequest("api/students/all", Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);

            List<Student> students = JsonConvert.DeserializeObject<List<Student>>(restResponse.Content);
           // List<Student> students = new List<Student>();// JsonConvert.DeserializeObject<List<Student>>(restResponse.Content);
            return View(students);
        }
    }
}
