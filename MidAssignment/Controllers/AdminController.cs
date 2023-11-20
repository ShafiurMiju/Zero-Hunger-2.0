using MidAssignment.Auth;
using MidAssignment.DTO;
using MidAssignment.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MidAssignment.Controllers
{
    public class AdminController : Controller
    {
        //-----Employee to EmployeeDTO-----//
        employeeDTO convertEmployee(Employee e)
        {
            return new employeeDTO()
            {
                EmployeeID = e.EmployeeID,
                Name = e.Name,
                Email = e.Email,
                Gender = e.Gender,
                ContactNumber = e.ContactNumber,
                Password = e.Password,
                Address = e.Address
            };
        }

        //-----EmployeeDTO to Employee-----//
        Employee convertEmployee(employeeDTO e)
        {
            return new Employee()
            {
                EmployeeID = e.EmployeeID,
                Name = e.Name,
                Email = e.Email,
                Gender = e.Gender,
                ContactNumber = e.ContactNumber,
                Password = e.Password,
                Address = e.Address
            };
        }

        List<employeeDTO> convertEmployee(List<Employee> e)
        {
            var list = new List<employeeDTO>();

            foreach (var et in e)
            {
                list.Add(convertEmployee(et));
            }
            return list;
        }

        //-----Restaurant to RestaurantDTO-----//
        restaurantDTO convertRestaurant(Restaurant r)
        {
            return new restaurantDTO()
            {
                RestaurantID = r.RestaurantID,
                Name = r.Name,
                ContactNumber = r.ContactNumber,
                Status = r.Status,
                Address = r.Address,
                Password = r.Password,
                Email = r.Email
            };
        }

        //-----RestaurantDTO to Restaurant-----//
        Restaurant convertRestaurant(restaurantDTO r)
        {
            return new Restaurant()
            {
                RestaurantID = r.RestaurantID,
                Name = r.Name,
                ContactNumber = r.ContactNumber,
                Status = r.Status,
                Address = r.Address,
                Password = r.Password,
                Email = r.Email
            };
        }

        List<restaurantDTO> convertRestaurant(List<Restaurant> r)
        {
            var list = new List<restaurantDTO>();

            foreach (var rt in r)
            {
                list.Add(convertRestaurant(rt));
            }
            return list;
        }

        //Employee list
        [HttpGet]
        [ALogged]
        public ActionResult employee()
        {

            var db = new MidAssignmentEntities();

            var data = convertEmployee(db.Employees.ToList());

            return View(data);
        }

        [HttpGet]
        [ALogged]
        public ActionResult addEmployee()
        {
            return View();
        }

        //Employee add
        [HttpPost]
        [ALogged]
        public ActionResult addEmployee(employeeDTO e)
        {
            var db = new MidAssignmentEntities();

            if (ModelState.IsValid)
            {
                db.Employees.Add(convertEmployee(e));
                db.SaveChanges();

                return RedirectToAction("employee");
            }
            return View(e);
        }

        //Employee Delete
        [ALogged]
        public ActionResult employeedelete(int id)
        {
            var db = new MidAssignmentEntities();

            var data = db.Employees.Find(id);

            db.Employees.Remove(data);

            db.SaveChanges();

            return RedirectToAction("employee");
        }

        //Restaurant list
        [HttpGet]
        [ALogged]
        public ActionResult restaurant()
        {

            var db = new MidAssignmentEntities();

            var restaurant = db.Restaurants.ToList();

            return View(restaurant);
        }

        [HttpGet]
        [ALogged]
        public ActionResult addrestaurant()
        {
            return View();
        }

        //Restaurant add
        [HttpPost]
        [ALogged]
        public ActionResult addrestaurant(restaurantDTO r)
        {
            var db = new MidAssignmentEntities();

            r.Status = "Active";

            if (ModelState.IsValid)
            {
                db.Restaurants.Add(convertRestaurant(r));
                db.SaveChanges();

                return RedirectToAction("restaurant");
            }
            return View(r);
        }

        // Restaurant delete
        [ALogged]
        public ActionResult Restaurantdelete(int id)
        {
            var db = new MidAssignmentEntities();

            var data = db.Restaurants.Find(id);

            db.Restaurants.Remove(data);

            db.SaveChanges();

            return RedirectToAction("restaurant");
        }


        //Show All Pending Request
        [HttpGet]
        [ALogged]
        public ActionResult pendingRequest(CollectRequest cr) 
        {
            var db = new MidAssignmentEntities();

            var pending = db.CollectRequests.ToList();

            ViewBag.Restaurants = db.Restaurants.ToList();
            ViewBag.FoodItems = db.FoodItems.ToList();
            ViewBag.Employees = db.Employees.ToList();

            return View(pending);
        }

        // Distribution
        [ALogged]
        [HttpGet]
        public ActionResult distribution()
        {
            var db = new MidAssignmentEntities();

            var d = db.FoodDistributions.ToList();

            ViewBag.Restaurants = db.Restaurants.ToList();
            ViewBag.FoodItems = db.FoodItems.ToList();
            ViewBag.CollectRequests = db.CollectRequests.ToList();
            ViewBag.Employees = db.Employees.ToList();

            return View(d);
        }
    }
}