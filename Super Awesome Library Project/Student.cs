using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseServer
{
    public class Student
    {
        public uint acctNo { get; set; }
        public uint pin { get; set; }
        public int balance { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public string profile { get; set; }

        public Student()
        {
            acctNo = 0;
            pin = 0;
            balance = 0;
            firstName = "";
            lastName = "";
            profile = "";
        }

        public Student(uint pin, uint acctNo, string firstName, string lastName, int balance, string profile)
        {
            this.pin = pin;
            this.acctNo = acctNo;
            this.firstName = firstName;
            this.lastName = lastName;
            this.balance = balance;
            this.profile = profile;
        }
    }
}
