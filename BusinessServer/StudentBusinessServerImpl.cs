using DatabaseServer;
using System;
using System.Drawing;
using System.ServiceModel;
using System.Threading;

namespace BusinessServer
{
    internal class StudentBusinessServerImpl : StudentBusinessServerInterface
    {
        private StudentServerInterface foob;

        private LogClass log;

        public StudentBusinessServerImpl()
        {
            ChannelFactory<StudentServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8100/DatabaseServer";
            foobFactory = new ChannelFactory<StudentServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
            log = new LogClass();

            log.Log("[BusinessServer.StudentBusinessServerImpl]: Initialize DatabaseServer");
        }

        public int GetNumEntries()
        {
            log.Log("[BusinessServer.GetNumEntries]: GetNumEntries was invoked");
            return foob.GetNumEntries();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int balance, out string firstName, out string lastName, out Bitmap profileBitmap)
        {
            log.Log("[BusinessServer.GetValuesForEntry]: GetValuesForEntry was invoked with index = " + index);
            try
            {
                foob.GetValuesForEntry(index, out acctNo, out pin, out balance, out firstName, out lastName, out profileBitmap);
            }
            catch (FaultException<ArgumentOutOfRangeException> oe)
            {
                Console.WriteLine(oe.Message);
                log.Log(String.Format("[BusinessServer.GetValuesForEntry]: Parameter index[{0}] is out of range.", index));
                // This FaultException<T> exception results in a fault being sent to the client.
                throw new FaultException<ArgumentOutOfRangeException>(new ArgumentOutOfRangeException("", oe.Message), "Parameter index is out of range.");
            }
        }

        public void GetValuesForSearch(string searchText, out uint stuAcctNo, out uint stuPin, out int stuBalance, out string stuFirstName, out string stuLastName, out Bitmap stuProfileBitmap)
        {
            log.Log("[BusinessServer.GetValuesForSearch]: GetValuesForSearch was invoked with searchText = " + searchText);
            stuAcctNo = 0;
            stuPin = 0;
            stuBalance = 0;
            stuFirstName = null;
            stuLastName = null;
            stuProfileBitmap = null;
            int numEntry = foob.GetNumEntries();
            for (int index = 1; index <= numEntry; index++)
            {
                string firstName, lastName;
                int balance;
                uint acctNo, pin;
                Bitmap profileBitmap;
                foob.GetValuesForEntry(index, out acctNo, out pin, out balance, out firstName, out lastName, out profileBitmap);
                if (firstName.ToLower().Contains(searchText.ToLower()))
                {
                    stuAcctNo = acctNo;
                    stuPin = pin;
                    stuBalance = balance;
                    stuFirstName = firstName;
                    stuLastName = lastName;
                    stuProfileBitmap = profileBitmap;
                    log.Log("[BusinessServer.GetValuesForSearch]: student[acctNo=" + acctNo + "] was found with firstName = " + firstName);
                    break;
                }
                
            }

            Random random = new Random();
            int seconds =  random.Next(1,5);
            log.Log("[BusinessServer.GetValuesForSearch]: Thread.Sleep for " + seconds + " seconds");
            Thread.Sleep(1000 * seconds); //Forced sleep for 1-4 seconds
            if (null == stuFirstName)
            {
                log.Log("[BusinessServer.GetValuesForSearch]: Student cannot be found with firstName = " + searchText);
            }
        }
    }
}