using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRUDApplication.Models;

namespace CRUDApplication.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEmployees()
        {
            using (CRUDApplicationEntities dc = new CRUDApplicationEntities())
            {
                var employees = dc.Employees.OrderBy(a => a.FirstName).ToList();
                return Json(new { data = employees }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Save(int id)
        {
            using(CRUDApplicationEntities dc = new CRUDApplicationEntities())
            {
                var v = dc.Employees.Where(a => a.EmployeeID == id).FirstOrDefault();
                return View(v);
            }
        }

        [HttpPost]
        public ActionResult Save(Employee e)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (CRUDApplicationEntities dc = new CRUDApplicationEntities())
                {
                    if (e.EmployeeID > 0)
                    {
                        // Edit
                        var v = dc.Employees.Where(a => a.EmployeeID == e.EmployeeID).FirstOrDefault();
                        if (v != null)
                        {
                            v.FirstName = e.FirstName;
                            v.LastName = e.LastName;
                            v.EmailID = e.EmailID;
                            v.City = e.City;
                            v.Country = e.Country;
                        }
                    } else
                    {
                        // Add
                        dc.Employees.Add(e);
                    }
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (CRUDApplicationEntities dc = new CRUDApplicationEntities())
            {
                var v = dc.Employees.Where(a => a.EmployeeID == id).FirstOrDefault();
                if (v != null)
                {
                    return View(v);
                } else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteEmployee(int id)
        {
            bool status = false;
            using(CRUDApplicationEntities dc = new CRUDApplicationEntities())
            {
                var v = dc.Employees.Where(a => a.EmployeeID == id).FirstOrDefault();
                if (v != null)
                {
                    dc.Employees.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }

            return new JsonResult { Data = new { status = status } };
        }
    }
}