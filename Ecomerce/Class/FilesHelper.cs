using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Ecomerce.Class
{
    public class FilesHelper
    {
        public static string UploadPhoto(HttpPostedFileBase file, string folder)
        {
            string Ruta = string.Empty;
            string pic = string.Empty;

            if (file != null)
            {
                pic = Path.GetFileName(file.FileName);
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

    }
}