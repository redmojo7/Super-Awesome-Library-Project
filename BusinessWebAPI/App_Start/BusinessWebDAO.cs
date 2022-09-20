using API_Classes;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Results;

namespace BusinessWebAPI.App_Start
{
    public class BusinessWebDAO
    {
        string URL = "http://localhost:51491/";

        internal string GenerateDB()
        {
            string result = "GenerateDB fail!";
            RestClient client = new RestClient(URL);
            RestRequest restRequest = new RestRequest("api/Students/GenerateDB", Method.Get);
            RestResponse restResponse = client.Execute(restRequest);
            if (restResponse.IsSuccessful)
            {
                result = "GenerateDB Successful!";
            }
            return result;
        }

        internal Bitmap GetAvatar(string path)
        {
            // send http request to search
            Bitmap profile;
            RestClient client = new RestClient(URL);
            RestRequest restRequest = new RestRequest("api/Students/profile", Method.Get);
            restRequest.AddParameter("path", path);
            byte[] bitmapdata = client.DownloadData(restRequest);
            using (var ms = new MemoryStream(bitmapdata))
            {
                profile = new Bitmap(ms);
            }
            return profile;
        }

        internal int GetNumEntries()
        {
            
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest("api/Students", Method.Get);
            RestResponse response = client.Execute(request);
            List<Student> students = JsonConvert.DeserializeObject<List<Student>>(response.Content);
            int num = 0;
            if (students != null)
                num = students.Count;
            
            return num;
        }

        internal string Delete(uint acctNo)
        {
            string result = "Error";
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest("api/Students/{id}", Method.Delete);
            request.AddUrlSegment("id", acctNo);
            RestResponse response = client.Execute(request);
            if (response != null)
            {
                if (response.IsSuccessful)
                {
                    result = "success";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    result = "NotFound";
                }
                // not exist
            }
            return result;
        }

        internal Student GetValuesForEntry(int id)
        {
            Student student = null;
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest("api/Students/{id}", Method.Get);
            request.AddUrlSegment("id", id);
            RestResponse response = client.Execute(request);
            
            if (response != null)
            {
                if (response.IsSuccessful)
                {
                    student = JsonConvert.DeserializeObject<Student>(response.Content);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception(id + "was not found");
                }
                // not exist
            }
            return student;
        }

        internal DataIntermed GetValuesForSearch(string searchStr)
        {
            //throw new NotImplementedException();
            return null;
        }

        internal string Insert(Student student)
        {
            string result = "Error";
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest("api/Students", Method.Post);
            request.AddBody(student);
            RestResponse response = client.Execute(request);
            if (response != null) {
                if (response.IsSuccessful)
                {
                    result = "success";
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    result = "Conflict";
                }
            }
            return result;
        }

        internal string Update(Student student)
        {
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest("api/Students/{id}", Method.Put);
            if (student.Id != null)
            {
                
               request.AddUrlSegment("id", student.Id);
            }
            request.AddBody(student);
            RestResponse response = client.Execute(request);
            return "";
        }
    }
}