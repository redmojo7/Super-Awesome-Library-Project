using DatabaseServer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDatabase
{
    public class Database
    {
        private readonly DataGenerator dataGenerator = new DataGenerator();
        
        private List<Student> students;

        // DatabaseClass constructor
        public Database()
        {
            //Initialize the database
            uint acctNo = 0, pin = 0;
            string firstName = "", lastName = "";
            int balance = 0;
            string profile = "";
            students = new List<Student>();
            for (uint i = 0; i < dataGenerator.RECORDS_NUMBER; i++) {
                dataGenerator.GetNextAccount(out pin, out acctNo, out firstName, out lastName, out balance, out profile);
                students.Add(new Student(pin, acctNo, firstName, lastName, balance, profile));
            }
        }

        public uint GetAcctNoByIndex(int index)
        {
            return students[index].acctNo;
        }

        public uint GetPINByIndex(int index)
        {
            return students[index].pin;
        }

        public string GetFirstNameByIndex(int index)
        {
            return students[index].firstName;

        }

        public string GetLastNameByIndex(int index)
        {
            return students[index].lastName;
        }

        public String GetProfileByIndex(int index)
        {
            return students[index].profile;
        }

        public int GetBalanceByIndex(int index)
        {
            return students[index].balance;
        }

        public int GetNumRecords()
        {
            return students.Count;
        }

    }

}
