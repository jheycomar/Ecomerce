using Ecomerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public static List<City> GetCities()
        {
            var cities = db.Cities.ToList();

            cities.Add(new City { CityId = 0, Name = "[Select a city...]" });

            return cities.OrderBy(d => d.Name).ToList();
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
    }
}