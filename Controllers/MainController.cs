using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.db;
using ZeroHunger.Models;

namespace ZeroHunger.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home() // Homepage for all
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddRestaurant() // add new restaurant(front)
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRestaurant(Restaurant data) // add new restaurant(back)
        {
            var db = new ZeroHungerEntities();
            db.Restaurants.Add(data);
            db.SaveChanges();
            return RedirectToAction("ViewRestaurant");
        }

        public ActionResult ViewRestaurant() // view all registered restaurants
        {
            var db = new ZeroHungerEntities();
            var data = (from r in db.Restaurants select r).ToList();
            return View(data);
        }

        public ActionResult DeleteRestaurant(int id) // delete a restaurant
        {
            var db = new ZeroHungerEntities();
            var data = (from r in db.Restaurants where r.r_id == id select r).SingleOrDefault();
            db.Restaurants.Remove(data);
            db.SaveChanges();
            return RedirectToAction("ViewRestaurant");
        }

        [HttpGet]
        public ActionResult AddEmployee() // add new employee(front)
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEmployee(Employee data) // add new employee(back)
        {
            var db = new ZeroHungerEntities();
            db.Employees.Add(data);
            db.SaveChanges();
            return RedirectToAction("ViewEmployee");
        }

        public ActionResult ViewEmployee() // view all employees
        {
            var db = new ZeroHungerEntities();
            var data = (from e in db.Employees select e).ToList();
            return View(data);
        }

        public ActionResult DeleteEmployee(int id) // delete an employee
        {
            var db = new ZeroHungerEntities();
            var data = (from r in db.Employees where r.e_id == id select r).SingleOrDefault();
            db.Employees.Remove(data);
            db.SaveChanges();
            return RedirectToAction("ViewRestaurant");
        }

        [HttpGet]
        public ActionResult AddAdmin() // add new admin(front)
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAdmin(Admin data) // add new admin(back)
        {
            var db = new ZeroHungerEntities();
            db.Admins.Add(data);
            db.SaveChanges();
            return RedirectToAction("ViewAdmin");
        }

        public ActionResult ViewAdmin() // view all admins
        {
            var db = new ZeroHungerEntities();
            var data = (from a in db.Admins select a).ToList();
            return View(data);
        }

        public ActionResult DeleteAdmin(int id) // delete an admin
        {
            var db = new ZeroHungerEntities();
            var data = (from r in db.Admins where r.a_id == id select r).SingleOrDefault();
            db.Admins.Remove(data);
            db.SaveChanges();
            return RedirectToAction("ViewRestaurant");
        }

        [HttpGet]
        public ActionResult Login() // login page for all(front)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password, string Type) // login(back)
        {
            var db = new ZeroHungerEntities();
            if (Type == "Admin")
            {
                var data = (from d in db.Admins
                            where d.email == email
                            where d.password == password
                            select d).SingleOrDefault();

                if (data!=null)
                {
                    Session["account"] = data.a_id;
                    return RedirectToAction("DashAdmin");
                }
            }
            else if (Type == "Employee")
            {
                var data = (from d in db.Employees
                            where d.email == email
                            where d.password == password
                            select d).SingleOrDefault();

                if (data != null)
                {
                    Session["account"] = data.e_id;
                    return RedirectToAction("DashEmployee");
                }
            }
            else if(Type == "Restaurant")
            {
                var data = (from d in db.Restaurants
                            where d.email == email
                            where d.password == password
                            select d).SingleOrDefault();

                if (data != null)
                {
                    Session["account"] = data.r_id;
                    return RedirectToAction("DashRestaurant");
                }
            }
            return RedirectToAction("Login");
        }

        public ActionResult DashRestaurant() // restaurant dashboard(front)
        {
            return View();
        }

        public ActionResult MakeRequest() // restaurant request form(front)
        {
            return View();
        }

        [HttpPost]
        public ActionResult MakeRequest(Request data)
        {
            var db = new ZeroHungerEntities();
            var req = new Request();
            req.items = data.items;
            var date = data.exp_date.ToString("yyyy-MM-dd");
            req.exp_date = Convert.ToDateTime(date);
            req.r_id = Convert.ToInt32(Session["account"]);
            req.status = "Pending";
            db.Requests.Add(req);
            db.SaveChanges();
            return RedirectToAction("DashRestaurant");
        }

        public ActionResult ShowRequest() // show all requests made by that restaurant
        {
            var id = Convert.ToInt32(Session["account"]);
            var db = new ZeroHungerEntities();
            var data = (from d in db.Requests where d.r_id == id select d).ToList();
            return View(data);
        }

        public ActionResult DeleteRequest(int id) // deletes a request from restaurant end
        {
            var db = new ZeroHungerEntities();
            var data = (from d in db.Requests where d.req_id == id select d).SingleOrDefault();
            db.Requests.Remove(data);
            db.SaveChanges();
            return RedirectToAction("ShowRequest");
        }

        public ActionResult DashAdmin() // Dashboard for admin
        {
            return View();
        }

        public ActionResult AssignEmployee() // Table to assign employee
        {
            var db = new ZeroHungerEntities();
            var data = (from d in db.Requests select d).ToList();
            return View(data);
        }

        public ActionResult AssignEmp(int id) // Employee assigning (front)
        {
            var db = new ZeroHungerEntities();
            var req = (from d in db.Requests where d.req_id == id select d).SingleOrDefault();
            var emp = (from e in db.Employees select e).ToList();
            var comb = new ReqEmp
            {
                Request = req,
                Employee = emp
            };
            return View(comb);
        }

        [HttpPost]
        public ActionResult AssignEmp(Request data) // Employee assigning (back)
        {
            var db = new ZeroHungerEntities();
            var req = (from d in db.Requests where d.req_id == data.req_id select d).SingleOrDefault();
            req.e_id = data.e_id;
            db.SaveChanges();
            return RedirectToAction("DashAdmin");
        }

        public ActionResult AllRequests() // Shows all the requests to admin
        {
            var db = new ZeroHungerEntities();
            var data = (from d in db.Requests select d).ToList();
            return View(data);
        }

        public ActionResult DashEmployee() // Dashboard for Employee
        {
            return View();
        }

        public ActionResult ChangeStatus() // table to change status for employee
        {
            var db = new ZeroHungerEntities();
            var id = Convert.ToInt32(Session["account"]);
            var data = (from d in db.Requests
                        where d.e_id == id
                        where d.status == "Pending"
                        select d).ToList();
            return View(data);
        }

        public ActionResult IsCompleted(int id) // Action to change status
        {
            var db = new ZeroHungerEntities();
            var data = (from d in db.Requests where d.req_id == id select d).SingleOrDefault();
            data.status = "Completed";
            db.SaveChanges();
            return RedirectToAction("CompletedRequests");
        }

        public ActionResult CompletedRequests() // To show all completed requests by that employee
        {
            var db = new ZeroHungerEntities();
            var id = Convert.ToInt32(Session["account"]);
            var data = (from d in db.Requests
                        where d.e_id == id
                        where d.status == "Completed"
                        select d).ToList();
            return View(data);
        }

        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("login");
        }

    }
}