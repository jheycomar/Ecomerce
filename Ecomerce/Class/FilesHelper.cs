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

        public static string UploadPhotoUser(string tabla, string campo)
        {
            string Ruta = string.Empty;
            string pic = "WA";
            string respo = "";
            string fecha = DateTime.Now.ToString("yyyyMMdd");
            if (tabla != null && campo != null)
            {
                if (tabla == "Companies")
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    try
                    {
                        var consulta = from Company in db.Companies select new { logname = Company.Logo, };
                        foreach (var Info in consulta)
                        {
                            respo = Convert.ToString(Info.logname.Substring(31));
                            respo = respo.Remove(4, 4);
                        }
                    }
                    catch (Exception)
                    { return null; }
                }

                if (tabla == "Users")
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    try
                    {
                        var consulta = from User in db.Users select new { logname = User.Photo, };
                        foreach (var Info in consulta)
                        {
                            respo = Convert.ToString(Info.logname.Substring(31));
                            respo = respo.Remove(4, 4);
                        }

                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }

                if (respo == string.Empty)
                { respo = Convert.ToString("0000"); }

                int hora = Convert.ToInt32(respo) + 1;
                pic = string.Format("{0}-{1}-{2}{3}.jpg", "IMG", fecha, pic, hora.ToString().PadLeft(4, '0'));
            }

            return pic;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}