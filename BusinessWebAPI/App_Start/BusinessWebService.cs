using API_Classes;
using DatabaseServer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web;

namespace BusinessWebAPI.App_Start
{
    public class BusinessWebService
    {
        public StudentServerInterface foob;
        public BusinessWebDAO businessWebDAO;


        public BusinessWebService()
        {
            ChannelFactory<StudentServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8100/DatabaseServer";
            foobFactory = new ChannelFactory<StudentServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

            businessWebDAO = new BusinessWebDAO();
        }

        internal void Delete(uint acctNo)
        {
            throw new NotImplementedException();
        }

        internal Bitmap getAvatar(uint acctNo)
        {
            businessWebDAO.getAvatar(acctNo);
            // do searrching
            Bitmap profileBitmap = null;
            int numEntry = foob.GetNumEntries();
            for (int index = 1; index <= numEntry; index++)
            {
                string firstName, lastName;
                int balance;
                uint acct, pin;

                foob.GetValuesForEntry(index, out acct, out pin, out balance, out firstName, out lastName, out profileBitmap);
                if (acct == acctNo)
                {
                    break;
                }
            }
            return profileBitmap;
        }

        internal int GenerateData()
        {
            int total;
            total = businessWebDAO.GetNumEntries();

            return foob.GetNumEntries();
        }

        internal DataIntermed GetValuesForEntry(int index)
        {
            DataIntermed s = businessWebDAO.GetValuesForEntry(index);

            uint acctNo = 0, pin = 0;
            int balance = 0;
            string firstName = null, lastName = null;
            Bitmap profileBitmap = null;
            foob.GetValuesForEntry(index, out acctNo, out pin, out balance, out firstName, out lastName, out profileBitmap);
            return new DataIntermed(pin, acctNo, firstName, lastName, balance, null);
        }

        internal DataIntermed GetValuesForSearch(string searchStr)
        {
            DataIntermed s = businessWebDAO.GetValuesForSearch(searchStr);

            DataIntermed student = null;
            int numEntry = foob.GetNumEntries();
            for (int index = 1; index <= numEntry; index++)
            {
                string firstName, lastName;
                int balance;
                uint acctNo, pin;
                Bitmap profileBitmap;
                foob.GetValuesForEntry(index, out acctNo, out pin, out balance, out firstName, out lastName, out profileBitmap);
                if (firstName.ToLower().Contains(searchStr.ToLower()))
                {
                    student = new DataIntermed(pin, acctNo, firstName, lastName, balance, null);
                    return student;
                }
            }
            return student;
        }

        internal void Insert(DataIntermed student)
        {
            throw new NotImplementedException();
        }

        internal void Update(DataIntermed student)
        {
            string result = businessWebDAO.insert(student);
        }

    }
}