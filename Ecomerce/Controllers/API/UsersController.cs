﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Ecomerce.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Ecomerce.Class;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Ecomerce.Controllers.API
{
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private EcomerceDataContext db = new EcomerceDataContext();

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Longin(JObject form)
        {
           // db.Configuration.ProxyCreationEnabled = false;
            string email = string.Empty;
            string password = string.Empty;
            dynamic jsonObject = form;

            try
            {
                email = jsonObject.Email.Value;
                password = jsonObject.Password.Value;
            }
            catch
            {
                return this.BadRequest("Incorrect call");
            }

            var userContext = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.Find(email, password);

            if (userASP == null)
            {
                return this.BadRequest("User or password wrong");
            }

            var user = db.Users.Where(u => u.UserName.ToLower() == email.ToLower())
                .Include(u => u.City)
                .Include(u => u.Department)
                .Include(u => u.Company).FirstOrDefault();

            if (user == null)
            {
                return this.BadRequest("User not found");
            }

            var userResponse = new UserResponse
            {
                Address = user.Address,
                CityId = user.CityId,
                CityName = user.City.Name,
                Company = user.Company,
                DepartmentId = user.DepartmentId,
                DepartmentName = user.Department.Name,
                FirstName = user.FirstName,
                IsAdmin = userManager.IsInRole(userASP.Id, "Admin"),
                IsCustomer = userManager.IsInRole(userASP.Id, "Customer"),
                IsSupplier = userManager.IsInRole(userASP.Id, "Supplier"),
                IsUser = userManager.IsInRole(userASP.Id, "User"),
                LastName = user.LastName,
                Phone = user.Phone,
                Photo = user.Photo,
                UserId = user.UserId,
                UserName = user.UserName,

            };

           
            return Ok(userResponse);
        }
        


        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
           // db.Configuration.ProxyCreationEnabled = false;
            return db.Users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
    }
}