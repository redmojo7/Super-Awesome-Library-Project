using API_Classes;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BusinessWebAPI.App_Start
{
    public class BusinessWebDAO
    {
        string URL = "http://localhost:53923/";


        internal void getAvatar(uint acctNo)
        {
            //throw new NotImplementedException();
        }

        internal int GetNumEntries()
        {
            /*
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest("api/Students", Method.Get);
            RestResponse response = client.Execute(request);
            List<Student> students = JsonConvert.DeserializeObject<List<Student>>(response.Content);
            int num = 0;
            if (students != null)
                num = students.Count;
            */
            return 100;
        }

        internal DataIntermed GetValuesForEntry(int index)
        {
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest("api/Students/{id}", Method.Get);
            request.AddUrlSegment("id", index);
            RestResponse response = client.Execute(request);
            Student student = JsonConvert.DeserializeObject<Student>(response.Content);
            return new DataIntermed(Convert.ToUInt32(student.Pin), Convert.ToUInt32(student.AccountNumber), student.FirstName, student.LastName, Convert.ToInt32(student.Balance), null);
        }

        internal DataIntermed GetValuesForSearch(string searchStr)
        {
            //throw new NotImplementedException();
            return null;
        }

        internal string insert(DataIntermed student)
        {
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest("api/Students", Method.Post);
            request.AddBody(new Student(student.firstName, student.lastName, student.balance, student.pin, student.acctNo));
            RestResponse response = client.Execute(request);
            return "";
        }

        internal string Update(DataIntermed student)
        {
            RestClient client = new RestClient(URL);
            RestRequest request = new RestRequest("api/Students/{id}", Method.Put);
            request.AddUrlSegment("id", student.acctNo);
            request.AddBody(new Student(student.firstName, student.lastName, student.balance, student.pin, student.acctNo));
            RestResponse response = client.Execute(request);
            return "";
        }
    }
}