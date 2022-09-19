using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessWebAPI
{
    public partial class Student
    {
        public Student(string firstName, string lastName, decimal? balance, decimal? accountNumber, decimal? pin)
        {
            FirstName = firstName;
            LastName = lastName;
            Balance = balance;
            AccountNumber = accountNumber;
            Pin = pin;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<decimal> AccountNumber { get; set; }
        public Nullable<decimal> Pin { get; set; }
        public string Avatar { get; set; }
    }
}