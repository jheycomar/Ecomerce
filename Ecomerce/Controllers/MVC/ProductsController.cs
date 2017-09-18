using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ecomerce.Models;
using Ecomerce.Class;

namespace Ecomerce.Controllers
{
    [Authorize(Roles ="User")]
    public class ProductsController : Controller
    {
        private EcomerceDataContext db = new EcomerceDataContext();

        // GET: Products
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var products = db.Products
                .Include(p => p.Category)
                .Include(p => p.Tax)
                .Where(p=>p.CompanyId==user.CompanyId);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            ViewBag.CategoryId = new SelectList(CombosHelper.GetCategories(user.CompanyId), "CategoryId", "Description");           
            ViewBag.TaxId = new SelectList(CombosHelper.GetTaxes(user.CompanyId), "TaxId", "Description");
            var product = new Product { CompanyId = user.CompanyId, };
            return View(product);
        }

        // POST: Products/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                var responsse = DBHelper.SaveChanges(db);
                if (!responsse.Succeded)
                {
                    ModelState.AddModelError(string.Empty, responsse.Message);
                    return View(product);
                }
                

                if (product.ImageFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Products";
                    pic = FilesHelper.GetNamePhoto(product.ProductId);
                    if (pic != null)
                    {
                        var response = FilesHelper.UploadPhoto(product.ImageFile, pic, folder);
                        product.Image = string.Format("{0}/{1}", folder, pic);
                        db.Entry(product).State = EntityState.Modified;
                        var respons = DBHelper.SaveChanges(db);
                        if (!responsse.Succeded)
                        {
                            ModelState.AddModelError(string.Empty, responsse.Message);
                            return View(product);
                        }
                       
                    }
                }
               
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(CombosHelper.GetCategories(user.CompanyId), "CategoryId", "Description", product.CategoryId);
            ViewBag.TaxId = new SelectList(CombosHelper.GetTaxes(user.CompanyId), "TaxId", "Description", product.TaxId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CombosHelper.GetCategories(product.CompanyId), "CategoryId", "Description", product.CategoryId);
            ViewBag.TaxId = new SelectList(CombosHelper.GetTaxes(product.CompanyId), "TaxId", "Description", product.TaxId);
            return View(product);
        }

        // POST: Products/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
           
            if (ModelState.IsValid)
            {
                var pic = product.Image;
                var folder = "~/Content/Products";

                if (product.ImageFile != null)
                {
                    if (pic == null || pic == string.Empty)
                    {
                        pic = FilesHelper.GetNamePhoto(product.ProductId);
                    }
                    else { pic = pic.Substring(19); }


                    if (pic != null)
                    {
                        pic = FilesHelper.UploadPhoto(product.ImageFile, pic, folder);
                        pic = string.Format("{0}/{1}", folder, pic);
                    }

                }
                product.Image = pic;
               
                    db.Entry(product).State = EntityState.Modified;
                    var respon = DBHelper.SaveChanges(db);
                    if (!respon.Succeded)
                    {
                        ModelState.AddModelError(string.Empty, respon.Message);
                        return View(product);
                    }                    
                    return RedirectToAction("Index");                
                
            }

            ViewBag.CategoryId = new SelectList(CombosHelper.GetCategories(product.CompanyId), "CategoryId", "Description", product.CategoryId);
            ViewBag.TaxId = new SelectList(CombosHelper.GetTaxes(product.CompanyId), "TaxId", "Description", product.TaxId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
           var respon = DBHelper.SaveChanges(db);
            if (!respon.Succeded)
            {
                ModelState.AddModelError(string.Empty, respon.Message);
                return View(product);
            }


            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
