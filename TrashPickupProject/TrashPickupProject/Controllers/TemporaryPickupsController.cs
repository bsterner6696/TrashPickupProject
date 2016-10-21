using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrashPickupProject.Models;
using Microsoft.AspNet.Identity;

namespace TrashPickupProject.Controllers
{
    public class TemporaryPickupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TemporaryPickups
        [Authorize]
        public ActionResult Index()
        {

            if (User.IsInRole("Admin"))
            {
                return View(db.TemporaryPickup.ToList()); 
            }
            else if (User.IsInRole("Customer"))
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                int customerId = -100000000;
                if (currentUser.HasCustomerDetails)
                {
                    Customer customer = db.Customer.FirstOrDefault(x => x.ApplicationUserId == currentUserId);
                    customerId = customer.Id;
                }

                    ViewBag.CurrentCustomerId = customerId;  

                return View("CustomerIndex", db.TemporaryPickup.ToList());
            }
            else
            {
                return HttpNotFound();
            }
        }

        // GET: TemporaryPickups/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemporaryPickup temporaryPickup = db.TemporaryPickup.Find(id);
            if (temporaryPickup == null)
            {
                return HttpNotFound();
            }
            return View(temporaryPickup);
        }

        // GET: TemporaryPickups/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customer, "Id", "StreetAddress");
            return View();
        }

        // POST: TemporaryPickups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StartDate,EndDate,DayOfWeek")] TemporaryPickup temporaryPickup)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                Customer customer = db.Customer.FirstOrDefault(x => x.ApplicationUserId == currentUserId);

                if (customer != null)
                {
                    temporaryPickup.Customer = customer;
                    temporaryPickup.CustomerId = customer.Id;
                    db.TemporaryPickup.Add(temporaryPickup);
                    db.SaveChanges();
                    return RedirectToAction("Index"); 
                }
                else
                {
                    RedirectToAction("Index");
                }
            }

            ViewBag.CustomerId = new SelectList(db.Customer, "Id", "StreetAddress", temporaryPickup.CustomerId);
            return View(temporaryPickup);
        }

        // GET: TemporaryPickups/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemporaryPickup temporaryPickup = db.TemporaryPickup.Find(id);
            if (temporaryPickup == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customer, "Id", "StreetAddress", temporaryPickup.CustomerId);
            return View(temporaryPickup);
        }

        // POST: TemporaryPickups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StartDate,EndDate,DayOfWeek,CustomerId")] TemporaryPickup temporaryPickup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(temporaryPickup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customer, "Id", "StreetAddress", temporaryPickup.CustomerId);
            return View(temporaryPickup);
        }

        // GET: TemporaryPickups/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TemporaryPickup temporaryPickup = db.TemporaryPickup.Find(id);
            if (temporaryPickup == null)
            {
                return HttpNotFound();
            }
            return View(temporaryPickup);
        }

        // POST: TemporaryPickups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TemporaryPickup temporaryPickup = db.TemporaryPickup.Find(id);
            db.TemporaryPickup.Remove(temporaryPickup);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
