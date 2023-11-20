using MidAssignment.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MidAssignment.Auth;
using System.Web.Helpers;

namespace MidAssignment.Controllers
{
    public class RestaurantController : Controller
    {
        // GET: Restaurant
        [RLogged]
        public ActionResult Index()
        {
            var db = new MidAssignmentEntities();

            var email = Session["Email"];
            var Id = db.Restaurants.Where(Restaurant => Restaurant.Email == email).Select(Restaurant => Restaurant.RestaurantID).FirstOrDefault();

            var data = db.CollectRequests.Where(CollectRequest => CollectRequest.RestaurantID == Id).ToList();

            ViewBag.Restaurants = db.Restaurants.ToList();
            ViewBag.Employees = db.Employees.ToList();

            return View(data);
        }

        //Request Delete
        [RLogged]
        public ActionResult requestdelete(int id)
        {
            var db = new MidAssignmentEntities();

            var data = db.CollectRequests.Find(id);

            db.CollectRequests.Remove(data);

            db.SaveChanges();

            return RedirectToAction("index");
        }

        [HttpGet]
        [RLogged]
        public ActionResult createRequest()
        {
            var db = new MidAssignmentEntities();

            var email = Session["Email"];
            var Id = db.Restaurants.Where(Restaurant => Restaurant.Email == email).Select(Restaurant => Restaurant.RestaurantID).FirstOrDefault();

            ViewBag.FoodItems = db.FoodItems.Where(FoodItem => FoodItem.RestaurantID == Id).ToList();

            return View();
        }

        [RLogged]
        [HttpPost]
        public ActionResult createRequest(CollectRequest cr)
        {
            var db = new MidAssignmentEntities();

            var email = Session["Email"];

            var Id = db.Restaurants.Where(Restaurant => Restaurant.Email == email).Select(Restaurant => Restaurant.RestaurantID).FirstOrDefault();

            cr.RestaurantID = Id;

            cr.Status = "Pending";
            

            if (ModelState.IsValid)
            {
                db.CollectRequests.Add(cr);
                db.SaveChanges();

                return RedirectToAction("index");
            }
            ViewBag.FoodItems = db.FoodItems.ToList();
            return View(cr);
        }

        

        [HttpGet]
        [RLogged]
        public ActionResult fooditems()
        {
            var db = new MidAssignmentEntities();
            var email = Session["Email"];
            var Id = db.Restaurants.Where(Restaurant => Restaurant.Email == email).Select(Restaurant => Restaurant.RestaurantID).FirstOrDefault();

            var data = db.FoodItems.Where(FoodItem => FoodItem.RestaurantID == Id).ToList();

            return View(data);
        }

        [HttpPost]
        [RLogged]
        public ActionResult fooditems(FoodItem d)
        {
            var db = new MidAssignmentEntities();

            var email = Session["Email"];

            var Id = db.Restaurants.Where(Restaurant => Restaurant.Email == email).Select(Restaurant => Restaurant.RestaurantID).FirstOrDefault();

            d.RestaurantID = Id;

            db.FoodItems.Add(d);

            db.SaveChanges();

            return RedirectToAction("fooditems");
        }

    }
}