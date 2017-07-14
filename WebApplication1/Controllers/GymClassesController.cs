using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class GymClassesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GymClasses
        public ActionResult Index()
        {
            return View(db.GymClasses.ToList());
        }

        // GET: GymClasses/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymClass gymClass = db.GymClasses.Find(id);
            if (gymClass == null)
            {
                return HttpNotFound();
            }
            return View(gymClass);
        }

        // GET: GymClasses/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                db.GymClasses.Add(gymClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gymClass);
        }

        // GET: GymClasses/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymClass gymClass = db.GymClasses.Find(id);
            if (gymClass == null)
            {
                return HttpNotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gymClass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GymClass gymClass = db.GymClasses.Find(id);
            if (gymClass == null)
            {
                return HttpNotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GymClass gymClass = db.GymClasses.Find(id);
            db.GymClasses.Remove(gymClass);
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

        [Authorize]
        public ActionResult BookingToggle(int id) {
            GymClass CurrentClass = db.GymClasses.Where(g => g.Id == id).FirstOrDefault();
            ApplicationUser CurrentUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if(CurrentClass.AttendingMembers.Contains(CurrentUser)) {
                CurrentClass.AttendingMembers.Remove(CurrentUser);
                db.SaveChanges();
            } else {
                CurrentClass.AttendingMembers.Add(CurrentUser);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult BookedPass() {
            ApplicationUser CurrentUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var bc = CurrentUser.AttendedClasses.Where(u => u.StartTime > DateTime.Now);

            //            GymClass CurrentClasses = db.GymClasses.Where(g => g.AttendingMembers..Contains(CurrentUser));

            //           if(CurrentClass.AttendingMembers.Contains(CurrentUser)) {
            //               CurrentClass.AttendingMembers.Remove(CurrentUser);
            ////               db.SaveChanges();
            //           } else {
            //               CurrentClass.AttendingMembers.Add(CurrentUser);
            //  //             db.SaveChanges();
            //           }



            return View(bc.ToList());
        }
        [Authorize]
        public ActionResult HistoryPass() {
            ApplicationUser CurrentUser = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

 //           var bc = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().AttendedClasses;
            var bc = CurrentUser.AttendedClasses.Where(u => u.StartTime < DateTime.Now);

            //            GymClass CurrentClasses = db.GymClasses.Where(g => g.AttendingMembers..Contains(CurrentUser));

            //           if(CurrentClass.AttendingMembers.Contains(CurrentUser)) {
            //               CurrentClass.AttendingMembers.Remove(CurrentUser);
            ////               db.SaveChanges();
            //           } else {
            //               CurrentClass.AttendingMembers.Add(CurrentUser);
            //  //             db.SaveChanges();
            //           }



            return View(bc.ToList());
        }
    }
}
