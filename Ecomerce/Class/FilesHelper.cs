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
        public static string UploadPhoto(HttpPostedFileBase file, string folder)
        {
            string Ruta = string.Empty;
            string pic = "WA";
            string respo="";        
            string fecha = DateTime.Now.ToString("yyyyMMdd");
            if (file != null)
            {
                try
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    var productos = from Company in db.Companies select new { CompanyId = Company.Logo, };
                    foreach (var productInfo in productos)
                    {
                        respo = Convert.ToString(productInfo.CompanyId.Substring(31));
                        respo = respo.Remove(4, 4);
                    }
                }
                catch (Exception)
                {
                    respo =Convert.ToString("0000");
                }               
                int  hora =Convert.ToInt32( respo)+1;
                pic = string.Format("{0}-{1}-{2}{3}.jpg", "IMG", fecha, pic, hora.ToString().PadLeft(4, '0'));             
                Ruta = Path.Combine(HttpContext.Current.Server.MapPath(folder), pic);
                file.SaveAs(Ruta);
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
            }

            return pic;
        }

       

        public void Dispose()
        {
            db.Dispose();
        }
    }
}