using System;
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
using Ecomerce.Class;
using System.Collections;
using System.Threading.Tasks;

namespace Ecomerce.Controllers.API
{
    public class ProductsController : ApiController
    {
        private EcomerceDataContext db = new EcomerceDataContext();


        //Get api/Products
        public async Task<IHttpActionResult> GetLeagues()
        {
            var res =await db.Products.ToArrayAsync();
            var list = new List<ProductResponse>();
            foreach (var product in res)
            {
                list.Add(new ProductResponse
                {
                    ProductId = product.ProductId,
                    Description = product.Description,
                    BarCode = product.BarCode,
                    Price = product.Price,
                    Image = product.Image,
                    Remarks = product.Remarks,
                    Stock = product.Stock,
                    Inventory = product.Inventories.ToList(),
                    Company = product.Company,
                    Category = product.Category,
                    Tax = product.Tax,
                   
                });
            }

            return Ok(list);
        }
        

        //public IQueryable<Product> GetProducts()
        //{
        //    db.Configuration.ProxyCreationEnabled = false;
        //    return db.Products;

        //}

       
            
        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductId == id) > 0;
        }
    }
}