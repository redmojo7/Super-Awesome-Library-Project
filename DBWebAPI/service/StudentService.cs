using DBWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBWebAPI.service
{
    public class StudentService
    {
        // Instantiate random number generator using system-supplied value as seed.
        readonly Random random = new Random();
        readonly public int RECORDS_NUMBER = 100;
        readonly List<string> firstNameList = new List<string> { "Latasha", "Ryan", "Heather", "Kevin", "Latasha", "Bryan", "Emily", "Whitney", "Matthew", "Amy" };
        readonly List<string> lastNameList = new List<string> { "Fields", "Sloan", "Hernandez", "Ruiz", "James", "Thompson", "Campbell", "Leach", "Sexton", "Heath" };
        readonly List<string> universityList = new List<string> { "Curtin University", "Harvard University", "Princeton University", "Columbia University", "Stanford University",
            "University of Oxford", "Yale University", "University of Chicago", "University of Michigan", "Boston University " };

        internal List<Student> GenerateDB()
        {
            List<Student> students = new List<Student>();
            for (uint i = 0; i < RECORDS_NUMBER; i++)
            {
                string firstName = firstNameList[random.Next(firstNameList.Count)];
                string lastName = lastNameList[random.Next(lastNameList.Count)];
                int pin = random.Next(random.Next(1000));
                int acctNum = (int)(i);
                int id = acctNum;
                int balance = random.Next(2000, 10000);
                string university = universityList[random.Next(universityList.Count)];
                string avatar = "bitmap-" + random.Next(10) + ".png";
                students.Add(new Student(id, firstName, lastName, pin, acctNum, balance, university, avatar));
            }
            return students;
        }
    }
}