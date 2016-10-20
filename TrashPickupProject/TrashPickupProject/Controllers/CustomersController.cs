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
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customers
        public ActionResult Index(string zipCode, string date, string day)
        {

            if (User.IsInRole("Admin"))
            {
                return View(db.Customer.ToList()); 
            }
            else if (User.IsInRole("Customer"))
            {
                string currentUserId = User.Identity.GetUserId();
                ViewBag.CurrentUserId = currentUserId;
                return View("CustomerIndex", db.Customer.ToList());
            }
            else if (User.IsInRole("TrashCollector"))
            {
                int ZipCode = 0;
                int.TryParse(zipCode, out ZipCode);
                DateTime Date = DateTime.Now;
                DateTime.TryParse(date, out Date);

                    var Customers = from x in db.Customer
                                    select x;
                    var TemporaryDates = from y in db.TemporaryPickup
                                         select y;
                    Customers = Customers.Where(x => x.Zipcode == ZipCode);
                    var PotentialTemporaryPickups = TemporaryDates.Where(y => y.StartDate < Date && y.EndDate > Date);
                    var PotentialTemporaryPickupsInZipcode = from x in Customers
                                                             join y in PotentialTemporaryPickups on x.Id equals y.CustomerId
                                                             select y;
                    var TemporaryPickupsInZipcodeToday = PotentialTemporaryPickupsInZipcode.Where(x => x.DayOfWeek == day);
                    var TemporaryPickupsInZipcodeNotToday = PotentialTemporaryPickupsInZipcode.Where(x => x.DayOfWeek != day);
                    var CustomersToday = Customers.Where(x => x.DayOfPickup == day);
                    var CustomersWithTemporaryPickupsTodayInZipcode = from x in TemporaryPickupsInZipcodeToday
                                                                      join y in Customers on x.CustomerId equals y.Id
                                                                      select y;
                    var CustomersWithTemporaryPickupsNotTodayInZipcode = from x in TemporaryPickupsInZipcodeNotToday
                                                                         join y in Customers on x.CustomerId equals y.Id
                                                                         select y;
                    var PossiblePickupsToday = CustomersToday.Union(CustomersWithTemporaryPickupsTodayInZipcode);
                    var ActualPickupsToday = PossiblePickupsToday.Except(CustomersWithTemporaryPickupsNotTodayInZipcode);

                    return View("EmployeeIndex", ActualPickupsToday);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [Authorize(Roles = "TrashCollector")]
        public ActionResult EmployeeIndex(string zipCode, string date, string day)
        {

            int ZipCode = 0;
            int.TryParse(zipCode, out ZipCode);
            DateTime Date = DateTime.Now;
            DateTime.TryParse(date, out Date);
            if (!string.IsNullOrEmpty(day) && date != null && ZipCode != 0)
            {
                var Customers = from x in db.Customer
                                select x;
                var TemporaryDates = from y in db.TemporaryPickup
                                     select y;
                Customers = Customers.Where(x => x.Zipcode == ZipCode);
                var PotentialTemporaryPickups = TemporaryDates.Where(y => y.StartDate < Date && y.EndDate > Date);
                var PotentialTemporaryPickupsInZipcode = from x in Customers
                                                         join y in PotentialTemporaryPickups on x.Id equals y.CustomerId
                                                         select y;
                var TemporaryPickupsInZipcodeToday = PotentialTemporaryPickupsInZipcode.Where(x => x.DayOfWeek == day);
                var TemporaryPickupsInZipcodeNotToday = PotentialTemporaryPickupsInZipcode.Where(x => x.DayOfWeek != day);
                var CustomersToday = Customers.Where(x => x.DayOfPickup == day);
                var CustomersWithTemporaryPickupsTodayInZipcode = from x in TemporaryPickupsInZipcodeToday
                                                                  join y in Customers on x.CustomerId equals y.Id
                                                                  select y;
                var CustomersWithTemporaryPickupsNotTodayInZipcode = from x in TemporaryPickupsInZipcodeNotToday
                                                                     join y in Customers on x.CustomerId equals y.Id
                                                                     select y;
                var PossiblePickupsToday = CustomersToday.Union(CustomersWithTemporaryPickupsTodayInZipcode);
                var ActualPickupsToday = PossiblePickupsToday.Except(CustomersWithTemporaryPickupsNotTodayInZipcode);
                return View("EmployeeIndex", ActualPickupsToday);
            }

            return HttpNotFound();
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        [Authorize(Roles = "Customer")]
        public ActionResult Create()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (!currentUser.HasCustomerDetails)
            {

                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Zipcode,StreetAddress,Name,DayOfPickup")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                customer.ApplicationUserId = currentUserId;
                customer.ApplicationUser = currentUser;
                db.Customer.Add(customer);
                currentUser.HasCustomerDetails = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            string currentUserId = User.Identity.GetUserId();
            if (customer.ApplicationUserId == currentUserId || User.IsInRole("Admin"))
            {
                if (customer == null)
                {
                    return HttpNotFound();
                }

                return View(customer); 
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Zipcode,StreetAddress,Name,DayOfPickup,ApplicationUser,ApplicationUserId")] Customer customer)
        {
            if (ModelState.IsValid)
            {

                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customer.Find(id);
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == customer.ApplicationUserId);
            currentUser.HasCustomerDetails = false;
            db.Customer.Remove(customer);
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
