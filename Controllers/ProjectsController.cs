using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TaskDone_V2.Models;

namespace TaskDone_V2.Controllers
{
    public class ProjectsController : Controller
    {
        private string FindUserEmail()
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByIdAsync(User.Identity.GetUserId()).Result;
            var userEmail = user.Email;
            return userEmail;
        }
        private void InactiveProjectStatus()
        {
            var projects = db.Projects.Where(p => p.StatusId == 1);
            (from p in projects
             where p.DateFinished < DateTime.Today
             select p).ToList().ForEach(x => x.StatusId = 2);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
        }
       private void ActiveProjectStatus()
        {
            var projects = db.Projects.Where(p => p.StatusId == 2);
            (from p in projects
             where p.DateFinished >= DateTime.Today
             select p).ToList().ForEach(x => x.StatusId = 1);
                      db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Projects
        public ActionResult Index(string sortOrder, string searchString)
        {
            InactiveProjectStatus();
            ActiveProjectStatus();

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "catName_desc" : "";
            ViewBag.PriceSortParm = String.IsNullOrEmpty(sortOrder) ? "Price_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var projects = db.Projects.Include(p => p.Category).Include(p => p.Status).Where(p => p.StatusId == 1);

            if (User.Identity.IsAuthenticated)
            {
                var currentUserEmail = FindUserEmail();
                var unlistedProject = projects.Where(p => p.UserEmail == currentUserEmail);
                projects = projects.Except(unlistedProject).Where(p=>p.StatusId==1);
            }
         
            if (!String.IsNullOrEmpty(searchString))
            {
                projects = projects.Where(s => s.Name.Contains(searchString)
                                       || s.Category.Name.Contains(searchString)
                                       || s.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    projects = projects.OrderByDescending(s => s.Name);
                    break;
                case "catName_desc":
                    projects = projects.OrderByDescending(s => s.Category.Name);
                    break;
                case "Price_desc":
                    projects = projects.OrderByDescending(s => s.ProposedPrice);
                    break;
                case "Date":
                    projects = projects.OrderBy(s => s.DateCreated);
                    break;
                case "date_desc":
                    projects = projects.OrderByDescending(s => s.DateCreated);
                    break;
                default:
                    projects = projects.OrderBy(s => s.Category.Name);
                    break;
            }
            
            return View(projects.ToList());
        }

        [Authorize]
        public ActionResult UserProjects(string sortOrder, string searchString)
        {
            InactiveProjectStatus();
            ActiveProjectStatus();

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "catName_desc" : "";
            ViewBag.PriceSortParm = String.IsNullOrEmpty(sortOrder) ? "Price_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.FinishDateSortParm = sortOrder == "f_Date" ? "f_date_desc" : "f_Date";

            var projects = db.Projects.Include(p => p.Category).Include(p => p.Status);
            string uEmail = FindUserEmail();
            projects = projects.Where(p => p.UserEmail == uEmail);

            if (!String.IsNullOrEmpty(searchString))
            {
                projects = projects.Where(s => s.Name.Contains(searchString)
                                       || s.Category.Name.Contains(searchString)
                                       || s.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    projects = projects.OrderByDescending(s => s.Name);
                    break;
                case "catName_desc":
                    projects = projects.OrderByDescending(s => s.Category.Name);
                    break;
                case "Price_desc":
                    projects = projects.OrderByDescending(s => s.ProposedPrice);
                    break;
                case "Date":
                    projects = projects.OrderBy(s => s.DateCreated);
                    break;
                case "date_desc":
                    projects = projects.OrderByDescending(s => s.DateCreated);
                    break;
                case "f_Date":
                    projects = projects.OrderBy(s => s.DateFinished);
                    break;
                case "f_date_desc":
                    projects = projects.OrderByDescending(s => s.DateFinished);
                    break;
                default:
                    projects = projects.OrderBy(s => s.Name);
                    break;
            }

            return View(projects.ToList());
        }


        [Authorize]
        // GET: Projects/Details/5
        public ActionResult UserProjectsInQueue(string sortOrder, string searchString)
        {
            InactiveProjectStatus();
            ActiveProjectStatus();

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "catName_desc" : "";
            ViewBag.PriceSortParm = String.IsNullOrEmpty(sortOrder) ? "Price_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.FinishDateSortParm = sortOrder == "f_Date" ? "f_date_desc" : "f_Date";

            var userEmail = FindUserEmail();
            var projects = db.Projects.Where(p=>p.UserEmail== userEmail && p.StatusId==4);


            if (!String.IsNullOrEmpty(searchString))
            {
                projects = projects.Where(s => s.Name.Contains(searchString)
                                       || s.Category.Name.Contains(searchString)
                                       || s.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    projects = projects.OrderByDescending(s => s.Name);
                    break;
                case "catName_desc":
                    projects = projects.OrderByDescending(s => s.Category.Name);
                    break;
                case "Price_desc":
                    projects = projects.OrderByDescending(s => s.ProposedPrice);
                    break;
                case "Date":
                    projects = projects.OrderBy(s => s.DateCreated);
                    break;
                case "date_desc":
                    projects = projects.OrderByDescending(s => s.DateCreated);
                    break;
                case "f_Date":
                    projects = projects.OrderBy(s => s.DateFinished);
                    break;
                case "f_date_desc":
                    projects = projects.OrderByDescending(s => s.DateFinished);
                    break;
                default:
                    projects = projects.OrderBy(s => s.Name);
                    break;
            }

            return View(projects.ToList());
        }

        [Authorize]
        // GET: Projects/Details/5
        public ActionResult UserProjectsCompleted(string sortOrder, string searchString)
        {
            InactiveProjectStatus();
            ActiveProjectStatus();

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "catName_desc" : "";
            ViewBag.PriceSortParm = String.IsNullOrEmpty(sortOrder) ? "Price_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.FinishDateSortParm = sortOrder == "f_Date" ? "f_date_desc" : "f_Date";

            var userEmail = FindUserEmail();
            var projects = db.Projects.Where(p => p.UserEmail == userEmail && p.StatusId == 3);


            if (!String.IsNullOrEmpty(searchString))
            {
                projects = projects.Where(s => s.Name.Contains(searchString)
                                       || s.Category.Name.Contains(searchString)
                                       || s.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    projects = projects.OrderByDescending(s => s.Name);
                    break;
                case "catName_desc":
                    projects = projects.OrderByDescending(s => s.Category.Name);
                    break;
                case "Price_desc":
                    projects = projects.OrderByDescending(s => s.ProposedPrice);
                    break;
                case "Date":
                    projects = projects.OrderBy(s => s.DateCreated);
                    break;
                case "date_desc":
                    projects = projects.OrderByDescending(s => s.DateCreated);
                    break;
                case "f_Date":
                    projects = projects.OrderBy(s => s.DateFinished);
                    break;
                case "f_date_desc":
                    projects = projects.OrderByDescending(s => s.DateFinished);
                    break;
                default:
                    projects = projects.OrderBy(s => s.Name);
                    break;
            }

            return View(projects.ToList());
        }

        [Authorize]
        public ActionResult MyCompletedProjects(string sortOrder, string searchString)
        {
            InactiveProjectStatus();
            ActiveProjectStatus();

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "catName_desc" : "";
            ViewBag.PriceSortParm = String.IsNullOrEmpty(sortOrder) ? "Price_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.FinishDateSortParm = sortOrder == "f_Date" ? "f_date_desc" : "f_Date";

            var userEmail = FindUserEmail();
            var projects = db.Offers.Where(o => o.UserEmail == userEmail).Select(o => o.Project).Where(p=>p.StatusId==3);

            if (!String.IsNullOrEmpty(searchString))
            {
                projects = projects.Where(s => s.Name.Contains(searchString)
                                       || s.Category.Name.Contains(searchString)
                                       || s.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    projects = projects.OrderByDescending(s => s.Name);
                    break;
                case "catName_desc":
                    projects = projects.OrderByDescending(s => s.Category.Name);
                    break;
                case "Price_desc":
                    projects = projects.OrderByDescending(s => s.ProposedPrice);
                    break;
                case "Date":
                    projects = projects.OrderBy(s => s.DateCreated);
                    break;
                case "date_desc":
                    projects = projects.OrderByDescending(s => s.DateCreated);
                    break;
                case "f_Date":
                    projects = projects.OrderBy(s => s.DateFinished);
                    break;
                case "f_date_desc":
                    projects = projects.OrderByDescending(s => s.DateFinished);
                    break;
                default:
                    projects = projects.OrderBy(s => s.Name);
                    break;
            }

            return View(projects.ToList());
        }

        [Authorize]
        public ActionResult MyAssignedProjectsInQueue(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "catName_desc" : "";
            ViewBag.PriceSortParm = String.IsNullOrEmpty(sortOrder) ? "Price_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.FinishDateSortParm = sortOrder == "f_Date" ? "f_date_desc" : "f_Date";

            var userEmail = FindUserEmail();
            var projects = db.Offers.Where(o => o.UserEmail == userEmail).Select(o=>o.Project).Where(p=>p.StatusId==4);

           

            if (!String.IsNullOrEmpty(searchString))
            {
                projects = projects.Where(s => s.Name.Contains(searchString)
                                       || s.Category.Name.Contains(searchString)
                                       || s.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    projects = projects.OrderByDescending(s => s.Name);
                    break;
                case "catName_desc":
                    projects = projects.OrderByDescending(s => s.Category.Name);
                    break;
                case "Price_desc":
                    projects = projects.OrderByDescending(s => s.ProposedPrice);
                    break;
                case "Date":
                    projects = projects.OrderBy(s => s.DateCreated);
                    break;
                case "date_desc":
                    projects = projects.OrderByDescending(s => s.DateCreated);
                    break;
                case "f_Date":
                    projects = projects.OrderBy(s => s.DateFinished);
                    break;
                case "f_date_desc":
                    projects = projects.OrderByDescending(s => s.DateFinished);
                    break;
                default:
                    projects = projects.OrderBy(s => s.Name);
                    break;
            }

            return View(projects.ToList());
        }
        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        public ActionResult ListProjectsPerCategory (int? id,string categoryName, string sortOrder, string searchString)
        {
            InactiveProjectStatus();
            ActiveProjectStatus();

            

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = String.IsNullOrEmpty(sortOrder) ? "Price_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var projects = db.Projects.Include(p => p.Category).Include(p => p.Status).Where(p=>p.StatusId==3);
           
            if (!String.IsNullOrEmpty(searchString))
            {
                projects = projects.Where(s => s.Name.Contains(searchString)
                                       || s.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    projects = projects.OrderByDescending(s => s.Name);
                    break;
                case "Price_desc":
                    projects = projects.OrderByDescending(s => s.ProposedPrice);
                    break;
                case "Date":
                    projects = projects.OrderBy(s => s.DateCreated);
                    break;
                case "date_desc":
                    projects = projects.OrderByDescending(s => s.DateCreated);
                    break;
                default:
                    projects = projects.OrderBy(s => s.Category.Name);
                    break;
            }
            
            ViewBag.CategoryName = categoryName;
            if (User.Identity.IsAuthenticated)
            {
                var currentUserEmail = FindUserEmail();
                return View(projects.Where(p => p.CategoryId == id && !p.UserEmail.Equals(currentUserEmail)).ToList());
            }
            return View(projects.ToList());
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
      
            ViewBag.CategoryId= new SelectList(db.Categories, "Id", "Name");
            ViewBag.StatusId = new SelectList(db.Status.Where(s => s.Id==1), "Id", "Name");
            Project project = new Project
            {
                DateCreated = DateTime.Today,
                UserEmail = FindUserEmail(),
               
            };
            return View(project);
        }

        // POST: Projects/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserEmail,Name,IsVolunteer,DateCreated,DateFinished,ProposedPrice,Description,CategoryId,StatusId")] Project project)
        {
            if (ModelState.IsValid)
            {
                              
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("UserProjects");
            }

            ViewBag.Category = new SelectList(db.Categories, "Id", "Name", project.CategoryId);
            ViewBag.Status = new SelectList(db.Status.Where(s => s.Id == 1), "Id", "Name");
            return View(project);
        }

        // GET: Projects/CompleteProject/5
        public ActionResult CompleteProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", project.CategoryId);
            ViewBag.StatusId = new SelectList(db.Status.Where(s=>s.Id==3), "Id", "Name", project.StatusId);
            return View(project);
        }

        // POST: Projects/CompleteProject/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteProject([Bind(Include = "Id,UserEmail,Name,IsVolunteer,DateCreated,DateFinished,ProposedPrice,Description,CategoryId,StatusId,AcceptedPrice")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                InactiveProjectStatus();
                ActiveProjectStatus();
                db.SaveChanges();
                return RedirectToAction("UserProjects");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", project.CategoryId);
            ViewBag.StatusId = new SelectList(db.Status, "Id", "Name", project.StatusId);

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", project.CategoryId);
            ViewBag.StatusId = new SelectList(db.Status, "Id", "Name", project.StatusId);
            return View(project);
        }

        // POST: Projects/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserEmail,Name,IsVolunteer,DateCreated,DateFinished,ProposedPrice,Description,CategoryId,StatusId,AcceptedPrice")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                InactiveProjectStatus();
                ActiveProjectStatus();
                db.SaveChanges();
                return RedirectToAction("UserProjects");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", project.CategoryId);
            ViewBag.StatusId = new SelectList(db.Status, "Id", "Name", project.StatusId);
            
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
           
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("UserProjects");
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
