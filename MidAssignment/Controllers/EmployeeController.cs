using MidAssignment.EF;
using MidAssignment.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MidAssignment.Controllers
{
    public class EmployeeController : Controller
    {
        // Pending List
        [HttpGet]
        [ELogged]
        public ActionResult Index()
        {
            var db = new MidAssignmentEntities();

            var pending = db.CollectRequests.Where(CollectRequest => CollectRequest.Status == "Pending").ToList(); 

            ViewBag.Restaurants = db.Restaurants.ToList();
            ViewBag.FoodItems = db.FoodItems.ToList();

            return View(pending);
        }

        // Collection List
        [HttpGet]
        [ELogged]
        public ActionResult collection() 
        {
            var db = new MidAssignmentEntities();

            var email = Session["Email"];
            var Id = db.Employees.Where(Employee => Employee.Email == email).Select(Employee => Employee.EmployeeID).FirstOrDefault();

            var pending = db.CollectRequests.Where(CollectRequest => CollectRequest.Status == "Accept" && CollectRequest.EmployeeID == Id).ToList();

            ViewBag.Restaurants = db.Restaurants.ToList();
            ViewBag.FoodItems = db.FoodItems.ToList();

            return View(pending);
        }

        // Collected List
        [HttpGet]
        [ELogged]
        public ActionResult collected()
        {
            var db = new MidAssignmentEntities();

            var email = Session["Email"];
            var Id = db.Employees.Where(Employee => Employee.Email == email).Select(Employee => Employee.EmployeeID).FirstOrDefault();

            var pending = db.FoodDistributions.Where(FoodDistribution => FoodDistribution.EmployeeID == Id).ToList();

            ViewBag.Restaurants = db.Restaurants.ToList();
            ViewBag.FoodItems = db.FoodItems.ToList();
            ViewBag.CollectRequests = db.CollectRequests.ToList();

            return View(pending);
        }

        // Request Update
        [ELogged]
        [HttpGet]
        public ActionResult requestupdate(int id)
        {
            var db = new MidAssignmentEntities();

            var data = db.CollectRequests.Find(id);

            return View(data);
        }

        [ELogged]
        [HttpPost]
        public ActionResult requestupdate(CollectRequest cr)
        {
            var db = new MidAssignmentEntities();

            var email = Session["Email"];
            var Id = db.Employees.Where(Employee => Employee.Email == email).Select(Employee => Employee.EmployeeID).FirstOrDefault();

            var data = db.CollectRequests.Find(cr.RequestID);

            data.Status = "Accept";
            data.EmployeeID = Id;

            db.SaveChanges();

            return RedirectToAction("index");
        }

        // Accepted Update
        [ELogged]
        [HttpGet]
        public ActionResult acceptupdate(int id)
        {
            var db = new MidAssignmentEntities();

            var data = db.CollectRequests.Find(id);

            return View(data);
        }

        [ELogged]
        [HttpPost]
        public ActionResult acceptupdate(CollectRequest cr)
        {
            var db = new MidAssignmentEntities();

            var email = Session["Email"];
            var Id = db.Employees.Where(Employee => Employee.Email == email).Select(Employee => Employee.EmployeeID).FirstOrDefault();

            var data = db.CollectRequests.Find(cr.RequestID);

            data.Status = "Collected";

            db.SaveChanges();


            var db1 = new MidAssignmentEntities();

            var fd = new FoodDistribution
            {
                RequestID = cr.RequestID,
                EmployeeID = Id,
                Status = "Not Distributed",
                
            };

            db1.FoodDistributions.Add(fd);
            db1.SaveChanges();


            return RedirectToAction("collection");
        }

        // Distributed Update
        [ELogged]
        [HttpGet]
        public ActionResult distributedupdate(int id)
        {
            var db = new MidAssignmentEntities();

            var data = db.FoodDistributions.Find(id);

            return View(data);
        }

        [ELogged]
        [HttpPost]
        public ActionResult distributedupdate(FoodDistribution fd)
        {
            var db = new MidAssignmentEntities();

            var data = db.FoodDistributions.Find(fd.DistributionID);

            data.Status = "Distributed";
            data.DistributionDate = DateTime.Now;

            db.SaveChanges();

            return RedirectToAction("collected");
        }

     
    }
}