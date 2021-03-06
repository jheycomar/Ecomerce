﻿using Ecomerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace Ecomerce.Class
{
    public class CombosHelper:IDisposable
    {
        private static EcomerceDataContext db = new EcomerceDataContext();
        public static List<Department> GetDepartments ()
        {
            var departments = db.Departments.ToList();
            departments.Add(new Department { DepartmentId = 0, Name = "[Select a department...]" });
            return departments.OrderBy(d => d.Name).ToList();

            
        }

        public static List<City> GetCities(int departmentId)
        {
            var cities = db.Cities.Where(c=>c.DepartmentId== departmentId).ToList();

            cities.Add(new City { CityId = 0, Name = "[Select a city...]" });

            return cities.OrderBy(d => d.Name).ToList();
        }

        public static List<Product> GetProducts(int companyId)
        {
            var products = db.Products.Where(p => p.CompanyId == companyId).ToList();

            products.Add(new Product { ProductId = 0, Description = "[Select a products...]" });

            return products.OrderBy(p => p.Description).ToList();
        }
        public static List<Product> GetProducts(int companyId, bool sw)
        {
            var products = db.Products.Where(p => p.CompanyId == companyId).ToList();
            return products.OrderBy(p => p.Description).ToList();
        }

        public static List<Company> GetCompanies()
        {
            var companies = db.Companies.ToList();

            companies.Add(new Company { CompanyId = 0, Name = "[Select a company...]" });

            return companies.OrderBy(d => d.Name).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public static List<Category> GetCategories(int companyId)
        {
            var categories = db.Categories.Where(c=>c.CompanyId==companyId).ToList();

            categories.Add(new Category { CategoryId = 0, Description = "[Select a Category...]" });

            return categories.OrderBy(d => d.Description).ToList();
        }

        public static List<Customer> GetCustomers(int companyId)
        {
            var qry = (from cu in db.Customers
                       join cc in db.CompanyCustomers on cu.CustomerId equals cc.CustomerId
                       join co in db.Companies on cc.CompanyId equals co.CompanyId
                       where co.CompanyId == companyId
                       select new { cu }).ToList();
            var customers = new List<Customer>();
            foreach (var item in qry)
            {
                customers.Add(item.cu);
            }

            customers.Add(new Customer { CustomerId = 0, FirstName = "[Select a customer...]" });

            return customers.OrderBy(d => d.FirstName).ThenBy(c=>c.LastName).ToList();
        }

        public static List<Tax> GetTaxes(int companyId)
        {
            var taxes = db.Taxes.Where(c => c.CompanyId == companyId).ToList();

            taxes.Add(new Tax { TaxId = 0, Description = "[Select a Tax...]" });

            return taxes.OrderBy(d => d.Description).ToList();
        }
    }
}