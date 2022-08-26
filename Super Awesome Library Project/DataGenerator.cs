using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDatabase
{
    internal class DataGenerator
    {
        // Instantiate random number generator using system-supplied value as seed.
        readonly Random random = new Random();
        readonly public int RECORDS_NUMBER = 10000;
        readonly List<string> firstNameList = new List<string> { "Latasha", "Ryan", "Heather", "Kevin", "Latasha", "Bryan", "Emily", "Whitney", "Matthew", "Amy" };
        readonly List<string> lastNameList = new List<string> { "Fields", "Sloan", "Hernandez", "Ruiz", "James", "Thompson", "Campbell", "Leach", "Sexton", "Heath" };
        
        private string GetFirstname()
        {
            return firstNameList[random.Next(firstNameList.Count)];
        }
        private string GetLastname()
        {
            return lastNameList[random.Next(lastNameList.Count)];
        }

        private uint GetPIN()
        {
            return (uint)random.Next(random.Next(1000));
        }

        private uint GetAcctNo()
        {
            return (uint)(100000 + random.Next(RECORDS_NUMBER));
        }
        
        private int GetBalance()
        {
            return random.Next(2000, 10000);
        }

        public void GetNextAccount(out uint pin, out uint acctNo, out string firstName, out string lastName, out int balance, out string profile)
        {
            pin = GetPIN();
            acctNo = GetAcctNo();
            firstName = GetFirstname();
            lastName = GetLastname();
            balance = GetBalance();
            profile = GetProfile();
        }

        private string GetProfile()
        {
            return "bitmap-" + random.Next(10) + ".png";
        }
    }
}
