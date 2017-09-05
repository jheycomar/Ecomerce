using Ecomerce.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecomerce.Class
{
    public class FilesHelper:IDisposable
    {
        private static EcomerceDataContext db = new EcomerceDataContext();
        public static string UploadPhoto(HttpPostedFileBase file, string name, string folder)
        {
            string Ruta = string.Empty;

            if (file != null)
              {
                Ruta = Path.Combine(HttpContext.Current.Server.MapPath(folder), name);
                file.SaveAs(Ruta);
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
            }

            return name;
        }

        public static string GetNamePhoto(int userId)
        {
            string pic = "WA";
            string fecha = DateTime.Now.ToString("yyyyMMdd");
            if (userId > 0 )
            {
                pic = string.Format("{0}-{1}-{2}{3}.jpg", "IMG", fecha, pic, userId.ToString().PadLeft(4, '0'));
            }

            return pic;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}