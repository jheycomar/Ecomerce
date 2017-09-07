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
    public class CustomersController : Controller
    {
        private EcomerceDataContext db = new EcomerceDataContext();

        // GET: Customers
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var customers = db.Customers.Where(c=>c.CompanyId==user.CompanyId).Include(c => c.City).Include(c => c.Department);
            return View(customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name");        
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name");
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var customer = new Customer { CompanyId=user.CompanyId};
            return View(customer);
        }

        // POST: Customers/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    UsersHelper.CreateUserASP(customer.UserName, "Customer");
                }
                #region catch Excepcion
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.Contains("_Index"))

                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same value");
                        ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name", customer.CityId);
                        ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", customer.DepartmentId);
                        return View(customer);

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                        ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name", customer.CityId);
                        ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", customer.DepartmentId);
                        return View(customer);
                    }

                } 
                #endregion

                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name", customer.CityId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", customer.DepartmentId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name", customer.CityId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", customer.DepartmentId);
            return View(customer);
        }

        // POST: Customers/Edit/5      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                //TODO: Validate when the customer email change

                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name", customer.CityId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", customer.DepartmentId);
            return View(customer);
        }

        // GET: Customers/Delete5/
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            try
            {
                db.SaveChanges();
                UsersHelper.DeleteUser(customer.UserName);
            }
            #region catch Excepcion
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.Contains("_Index"))

                {
                    ModelState.AddModelError(string.Empty, "There are a record with the same value");
                    ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name", customer.CityId);
                    ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", customer.DepartmentId);
                    return View(customer);

                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name", customer.CityId);
                    ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", customer.DepartmentId);
                    return View(customer);
                }

            }
            #endregion
            return RedirectToAction("Index");
        }
        public JsonResult GetCities(int departmentId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cities = db.Cities.Where(m => m.DepartmentId == departmentId);
            return Json(cities);
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
