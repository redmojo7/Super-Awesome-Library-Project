using System;
using System.ServiceModel;
using StudentDatabase;
using System.Drawing;
using System.IO;

namespace DatabaseServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class StudentServerImpl : StudentServerInterface
    {
        private const string resourcesPath = "Resources";
        Database database = null;

        public StudentServerImpl()
        {
            database = new Database();
        }
        public int GetNumEntries()
        {
            return database.GetNumRecords();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap profileBitmap)
        {
            try
            {
                int realIndex = index - 1;
                acctNo = database.GetAcctNoByIndex(realIndex);
                pin = database.GetPINByIndex(realIndex);
                bal = database.GetBalanceByIndex(realIndex);
                fName = database.GetFirstNameByIndex(realIndex);
                lName = database.GetLastNameByIndex(realIndex);

                string profileName = database.GetProfileByIndex(realIndex);
   
                string profileFullPath = Path.Combine(Directory.GetCurrentDirectory(), resourcesPath, profileName);
                
                // Retrieve the profile image.
                profileBitmap = new Bitmap(profileFullPath);

            }
            catch (ArgumentOutOfRangeException e)
            {
       
                // Index was out of range. Must be non-negative and less than the size of the collection.
                Console.WriteLine("ArgumentOutOfRangeException: {0}", e.Message);
                // This FaultException<T> exception results in a fault being sent to the client.
                throw new FaultException<ArgumentOutOfRangeException>(e, "Parameter index is out of range.");
            }
        }
    }
}
