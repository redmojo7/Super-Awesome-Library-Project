using System.Drawing;

namespace API_Classes
{
    public class DataIntermed
    {
        public uint acctNo { get; set; }
        public uint pin { get; set; }
        public int balance { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public Bitmap profile { get; set; }

        public DataIntermed()
        {
            acctNo = 0;
            pin = 0;
            balance = 0;
            firstName = "";
            lastName = "";
            profile = null;
        }

        public DataIntermed(uint pin, uint acctNo, string firstName, string lastName, int balance, Bitmap profile)
        {
            this.pin = pin;
            this.acctNo = acctNo;
            this.firstName = firstName;
            this.lastName = lastName;
            this.balance = balance;
            this.profile = profile;
        }

        public override string ToString()
        {
            string info = "The student's name is " + firstName + " " + lastName + "\n";
            info = info + "The student's acctNo is " + acctNo + "\n";
            return info;
        }
    }
}
