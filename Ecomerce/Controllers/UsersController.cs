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
    public class UsersController : Controller
    {
        private EcomerceDataContext db = new EcomerceDataContext();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.City).Include(u => u.Company).Include(u => u.Department);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(0), "CityId", "Name");
            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name");
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                 db.Users.Add(user);
                var responsse = DBHelper.SaveChanges(db);
                if (responsse.Succeded)
                {
                    UsersHelper.CreateUserASP(user.UserName, "User");

                }
                else
                {
                    ModelState.AddModelError(string.Empty, responsse.Message);
                    return View(user);
                }
               
                if (user.PhotoFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Users";
                    pic = FilesHelper.GetNamePhoto(user.UserId);
                    if (pic != null)
                    {
                       var  response = FilesHelper.UploadPhoto(user.PhotoFile, pic, folder);                           
                       user.Photo = string.Format("{0}/{1}", folder, pic);
                        db.Entry(user).State = EntityState.Modified;
                        var respons = DBHelper.SaveChanges(db);

                        if (!respons.Succeded)
                        {
                            ModelState.AddModelError(string.Empty, responsse.Message);
                            return View(user);
                        }
                     }
                } 
               
                  return RedirectToAction("Index");
                
            }

            ViewBag.CityId = new SelectList(CombosHelper.GetCities(user.DepartmentId), "CityId", "Name", user.CityId);
            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", user.DepartmentId);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(user.DepartmentId), "CityId", "Name", user.CityId);
            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", user.DepartmentId);
            return View(user);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var pic = user.Photo;
                var folder = "~/Content/Users";

                if (user.PhotoFile != null)
                {
                    if (pic ==null || pic ==string.Empty)
                    {
                        pic = FilesHelper.GetNamePhoto(user.UserId);
                    }
                    else { pic = pic.Substring(16); }
                   
                  
                    if (pic != null)
                    {
                        pic = FilesHelper.UploadPhoto(user.PhotoFile, pic, folder);
                        pic = string.Format("{0}/{1}", folder, pic);
                    }

                }
                user.Photo = pic;
                var db2 = new EcomerceDataContext();
                var currenrUser = db2.Users.Find(user.UserId);
                if (currenrUser.UserName != user.UserName)
                {
                    UsersHelper.UpdateUserName(currenrUser.UserName, user.UserName);
                }
                db2.Dispose();
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(user.DepartmentId), "CityId", "Name", user.CityId);
            ViewBag.CompanyId = new SelectList(CombosHelper.GetCompanies(), "CompanyId", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", user.DepartmentId);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            UsersHelper.DeleteUser(user.UserName,"User");
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
