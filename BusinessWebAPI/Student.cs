using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessWebAPI
{
    public partial class Student
    {
        public Student()
        {
            //Id = null;
        }

        public Student(string firstName, string lastName, int balance, int acctNum, int pin, string avatar)
        {
            //Id = null;
            FirstName = firstName;
            LastName = lastName;
            Balance = balance;
            AcctNum = acctNum;
            Id = acctNum;
            Pin = pin;
            Avatar = avatar;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> Balance { get; set; }
        public Nullable<int> AcctNum { get; set; }
        public Nullable<int> Pin { get; set; }
        public string Avatar { get; set; }
        public string University { get; set; }
    }
}