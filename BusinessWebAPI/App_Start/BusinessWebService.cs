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

        internal string Delete(uint Id)
        {
            return businessWebDAO.Delete(Id);
        }

        internal Bitmap getAvatar(uint acctNo)
        {
            businessWebDAO.GetAvatar(acctNo);
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

        internal string GenerateDB()
        {
            string result = "";
            //total = businessWebDAO.GenerateDB();

            return result;
        }

        internal Student GetValuesForEntry(int id)
        {
            return businessWebDAO.GetValuesForEntry(id);
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

        internal string Insert(Student student)
        {
            return businessWebDAO.Insert(student);
        }

        internal string Update(Student student)
        {
            return businessWebDAO.Update(student);
        }

        internal int GetNumEntries()
        {
            return businessWebDAO.GetNumEntries();
        }

        internal Student Get(int id)
        {
            return businessWebDAO.GetValuesForEntry(id);
        }
    }
}