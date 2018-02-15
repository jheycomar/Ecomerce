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
    [Authorize(Roles = "Admin")]
    public class CompaniesController : Controller
    {
        private EcomerceDataContext db = new EcomerceDataContext();

        // GET: Companies
        public ActionResult Index()
        {
            var companies = db.Companies.Include(c => c.City).Include(c => c.Department);
            return View(companies.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(0), "CityId", "Name");
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name");
            return View();
        }

        // POST: Companies/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Company company)
        {
            if (ModelState.IsValid)
             {                
                 db.Companies.Add(company);
                 var respons = DBHelper.SaveChanges(db);

                 if (!respons.Succeded)
                  {
                     ModelState.AddModelError(string.Empty, respons.Message);
                     return View(company);
                  }
               

                if (company.LogoFile != null)
                {

                    var folder = "~/Content/Logos";
                    var pic = FilesHelper.GetNamePhoto(company.CompanyId);
                    if (pic != null)
                    {
                        pic = FilesHelper.UploadPhoto(company.LogoFile, pic, folder);
                        company.Logo = string.Format("{0}/{1}", folder, pic);
                                               
                            db.Entry(company).State = EntityState.Modified;
                            var respon = DBHelper.SaveChanges(db);
                            if (!respon.Succeded)
                            {
                                ModelState.AddModelError(string.Empty, respon.Message);
                                return View(company);
                            }
                         
                    }
                }
                return RedirectToAction("Index");
                
            }

            ViewBag.CityId = new SelectList(CombosHelper.GetCities(company.DepartmentId), "CityId", "Name", company.CityId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", company.DepartmentId);
            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(company.DepartmentId), "CityId", "Name", company.CityId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", company.DepartmentId);
            return View(company);
        }

        // POST: Companies/Edit/5     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Company company)
        {
            if (ModelState.IsValid)
            {
                var pic = company.Logo;
                var folder = "~/Content/Logos";

                if (company.LogoFile != null)
                {
                    pic = pic.Substring(16);
                    pic = FilesHelper.UploadPhoto(company.LogoFile,pic, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                    company.Logo = pic;
                }
                db.Entry(company).State = EntityState.Modified;
                var respon = DBHelper.SaveChanges(db);
                if (!respon.Succeded)
                {
                 ModelState.AddModelError(string.Empty, respon.Message);
                  return View(company);
                }
                 return RedirectToAction("Index");
                
                
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(company.DepartmentId), "CityId", "Name", company.CityId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", company.DepartmentId);
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            var respon = DBHelper.SaveChanges(db);
            if (!respon.Succeded)
            {
                ModelState.AddModelError(string.Empty, respon.Message);
                return View(company);
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
