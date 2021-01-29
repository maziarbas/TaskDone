using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaskDone_V2.Models;

namespace TaskDone_V2.Controllers
{
    
    public class OffersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string FindUserEmail()
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByIdAsync(User.Identity.GetUserId()).Result;
            var userEmail = user.Email;
            return userEmail;
        }


        [Authorize]
        // GET: Offers
        public ActionResult ListProjectOffers(int id)
        {
            var userEmail = FindUserEmail();

            var offers = db.Offers.Include(o => o.Project);
            var selectedOffers = offers.Where(o => o.ProjectId == id);
            return View(selectedOffers.ToList());
        }

        [Authorize]
        public ActionResult UserOffers()
        {
            var offers = db.Offers.Include(o => o.Project);
            var useremail = FindUserEmail();
            var selectedOffers = offers.Where(o => o.UserEmail.Equals(useremail));
            return View(selectedOffers.ToList());
        }

        [Authorize]
        public ActionResult UserAcceptedOffers()
        {
            var userEmail = FindUserEmail();
            var offers = db.Offers.Include(o => o.Project).Where(o=>o.UserEmail!= userEmail);
            
            var selectedOffers = offers.Where(o=>o.Accepted == true && o.Project.UserEmail==userEmail);
            return View(selectedOffers.ToList());
        }
        [Authorize]
        public ActionResult OfferOnUserProjects()
        {
            var userEmail = FindUserEmail();
            var offers = db.Offers.Include(o => o.Project).Where(o => o.UserEmail != userEmail);
            var selectedOffers = offers.Where(o => o.Project.UserEmail == userEmail);
            return View(selectedOffers.ToList());
        }     

        [Authorize]
        public ActionResult MyAcceptedOffers()
        {
            var userEmail = FindUserEmail();
            var offers = db.Offers.Include(o => o.Project).Where(o => o.UserEmail == userEmail);
            var selectedOffers = offers.Where(o => o.Accepted == true);
            return View(selectedOffers.ToList());
        }

        [Authorize]
        // GET: Offers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }


        [Authorize]
        // GET: Offers/Create/5
        public ActionResult Create(int id)
        {
            
            var projects = db.Projects.Include(p => p.Category).Include(p => p.Status);
            var project = db.Projects.SingleOrDefault(r => r.Id == id);
            var projectUserEmail = project.UserEmail;
            Offer offer = new Offer();
           
            if (FindUserEmail()== projectUserEmail)
            {
                return RedirectToAction("ListProjectsPerCategory", "Projects");
            }

            ViewBag.ProjectName = project.Name;
            ViewBag.ProposedPrice= project.ProposedPrice;
            offer.ProjectId = id;
            offer.OfferedDate = DateTime.Today;
            offer.UserEmail = FindUserEmail();
            if(project.IsVolunteer)
            {
                offer.OfferedPrice = 0;
            }

            return View(offer);
        }


        [Authorize]
        // POST: Offers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserEmail,ProjectId,OfferedPrice,OfferedDate,Accepted")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                db.Offers.Add(offer);
                db.SaveChanges();
                return RedirectToAction("ListProjectsPerCategory", "Projects");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "UserEmail", offer.ProjectId);
            return View(offer);
        }


        [Authorize]
        // GET: Offers/Accept/5
        public ActionResult Accept(int? id)
        {
            ViewBag.StatusId = ViewBag.StatusId = new SelectList(db.Status.Where(s => s.Id == 4), "Id", "Name");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var offers = db.Offers.Include(o => o.Project).Include(p => p.Project.Status);
            Offer offer = offers.SingleOrDefault(r => r.Id == id);
           
            if (offer == null)
            {
                return HttpNotFound();
            }

            return View(offer);
        }


        // POST: Offers/Accept/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Accept([Bind(Include = "Id,UserEmail,ProjectId,OfferedPrice,OfferedDate,Accepted")] Offer offer)
        {

            if (offer == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                var projectId = offer.ProjectId;
                var projects = db.Projects.Include(p => p.Category).Include(p => p.Status);
                var status = db.Status;
                Project project = projects.SingleOrDefault(r => r.Id == projectId);
                project.AcceptedPrice = offer.OfferedPrice;
                var Status = status.Where(s=>s.Id==4).FirstOrDefault();
                project.Status =Status;
                
                db.Entry(project).State = EntityState.Modified;

                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                return RedirectToAction("ListProjectOffers", new { id = offer.ProjectId });
            }

            return View(offer);


        }


        [Authorize]
        // GET: Offers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "UserEmail", offer.ProjectId);
            return View(offer);
        }

        // POST: Offers/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserEmail,ProjectId,OfferedPrice,OfferedDate")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(offer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "UserEmail", offer.ProjectId);
            return View(offer);
        }

        // GET: Offers/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Offer offer = db.Offers.Find(id);
            db.Offers.Remove(offer);
            db.SaveChanges();
            return RedirectToAction("ListProjectOffers", new { id = offer.ProjectId });
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
