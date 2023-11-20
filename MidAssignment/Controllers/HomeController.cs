using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MidAssignment.Auth;
using MidAssignment.DTO;
using MidAssignment.EF;

namespace MidAssignment.Controllers
{
    public class HomeController : Controller
    {
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

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult join()
        {
            return View();
        }

        //Restaurant add
        [HttpPost]
        public ActionResult join(restaurantDTO r)
        {
            var db = new MidAssignmentEntities();

            r.Status = "Active";

            if (ModelState.IsValid)
            {
                db.Restaurants.Add(convertRestaurant(r));
                db.SaveChanges();

                return RedirectToAction("index");
            }
            return View(r);
        }

        //Login
        [HttpGet]
        public ActionResult login()
        {
            if (Session["Email"] != null && Session["User"] == "restaurant")
            {
                
                return RedirectToAction("Index", "restaurant");
            }
            else if (Session["Email"] != null && Session["User"] == "admin")
            {

                return RedirectToAction("employee", "admin");
            }
            else if (Session["Email"] != null && Session["User"] == "employee")
            {

                return RedirectToAction("index", "employee");
            }
            return View();

        }

        public ActionResult login(Restaurant r)
        {
            var db = new MidAssignmentEntities();

            if (ModelState.IsValid)
            {
                var restaurant = db.Restaurants.FirstOrDefault(email => email.Email == r.Email && email.Password == r.Password);
                var admin = db.Admins.FirstOrDefault(email => email.Email == r.Email && email.Password == r.Password);
                var employee = db.Employees.FirstOrDefault(email => email.Email == r.Email && email.Password == r.Password);

                if (restaurant != null)
                {
                    Session["Email"] = restaurant.Email;
                    Session["User"] = "restaurant";

                    return RedirectToAction("Index", "Restaurant");
                }
                else if (admin != null)
                {
                    Session["Email"] = admin.Email;
                    Session["User"] = "admin";

                    return RedirectToAction("employee", "Admin");
                }
                else if (employee != null)
                {
                    Session["Email"] = employee.Email;
                    Session["User"] = "employee";

                    return RedirectToAction("index", "employee");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login credentials");
                }
            }

            return View(r);

        }

        //Log Out
        public ActionResult Logout()
        {
            Session.Abandon();

            return RedirectToAction("login", "home");
        }

    }
}