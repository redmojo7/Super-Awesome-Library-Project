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
        public BusinessWebDAO businessWebDAO;


        public BusinessWebService()
        {
            businessWebDAO = new BusinessWebDAO();
        }

        internal string Delete(uint Id)
        {
            return businessWebDAO.Delete(Id);
        }

        internal Bitmap getAvatar(string path)
        {
            return businessWebDAO.GetAvatar(path);
        }

        internal string GenerateDB()
        {
            string result = "";
            businessWebDAO.GenerateDB();
            return result;
        }

        internal Student GetValuesForEntry(int id)
        {
            return businessWebDAO.GetValuesForEntry(id);
        }

        internal Student GetValuesForSearch(string searchStr)
        {
            return businessWebDAO.GetValuesForSearch(searchStr);
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

        internal string UploadAvarta(int id, HttpPostedFile file)
        {
            return businessWebDAO.UploadAvarta(id, file);
        }

        internal List<Student> All()
        {
            return businessWebDAO.All();
        }
    }
}