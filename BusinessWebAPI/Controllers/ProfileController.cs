using System;
using System.IO;
using System.Web.Http;
//using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Threading.Tasks;
using System.Web.Mvc;
using BusinessWebAPI.App_Start;
using System.Net.Http;
using RouteAttribute = System.Web.Http.RouteAttribute;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using System.Net;
using API_Classes;
using DatabaseServer;

namespace BusinessWebAPI.Controllers
{
    [Route("api/profile")]
    public class ProfileController : ApiController
    {

        private readonly BusinessWebService businessWebService;

        public ProfileController()
        {
            businessWebService = new BusinessWebService();
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetProfile(string path)
        {
            HttpResponseMessage response;
            if (path != null)
            {
                Bitmap profileBitmap = businessWebService.getAvatar(path);

                Byte[] b;
                using (var stream = new MemoryStream())
                {
                    profileBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    b = stream.ToArray();
                }
                response = new HttpResponseMessage();
                response.Content = new ByteArrayContent(b);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return response;
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                return response;
            }
        }

        [HttpGet]
        [Route("old")]
        public async Task<HttpResponseMessage> GetProfileOld(string acctNo = null)
        {

            int width = 100, height = 100;

            //bitmap
            Bitmap bmp = new Bitmap(width, height);

            //random number
            Random rand = new Random();

            //create random pixels
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //generate random ARGB value
                    int a = rand.Next(256);
                    int r = rand.Next(256);
                    int g = rand.Next(256);
                    int b = rand.Next(256);

                    //set ARGB value
                    bmp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }


            HttpResponseMessage response;
            if (!string.IsNullOrEmpty(acctNo))
            {

                Byte[] b;
                Bitmap profile = bmp;
                using (var stream = new MemoryStream())
                {
                    profile.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    b = stream.ToArray();
                }
                //return File(BitmapToBytes(profile), "image/jpeg");
                response = new HttpResponseMessage();
                response.Content = new ByteArrayContent(b);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return response;
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                return response;
            }
        }
    }
}
