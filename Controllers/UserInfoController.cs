using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaskDone_V2.Models;

namespace TaskDone_V2.Controllers
{
    
    public class UserInfoController : Controller
    {
        public Microsoft.AspNet.Identity.UserManager<ApplicationUser> UserManager { get; private set; }
        private ApplicationDbContext _context;

        public UserInfoController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: UserInfo
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByIdAsync(User.Identity.GetUserId()).Result; ;
            return View("Index", user);
        }
        public FileContentResult UserPhotos()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();

                if (userId == null)
                {
                    string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);

                    return File(imageData, "image/png");

                }
                // to get the user details to load user Image    
                var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();

                return new FileContentResult(userImage.UserPhoto, "image/jpeg/jpg");
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");

            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult RateHelpee(int id)
        {
            var project = _context.Projects.Include(p => p.Status).Include(p => p.Category).Where(p => p.Id == id).SingleOrDefault();
            var userEmail = project.UserEmail;
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByName(userEmail);

            return View("RateHelpee", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RateHelpee(ApplicationUser currentUser)
        {

            //Gets a value that indicates whether this instance received from the view is valid.
            if (!ModelState.IsValid)
            {
                return View(currentUser);

            }
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByIdAsync(User.Identity.GetUserId()).Result;

            user.FirstName = currentUser.FirstName;
            user.LastName = currentUser.LastName;
            user.SocialSecurity = currentUser.SocialSecurity;
            user.CompanyName = currentUser.CompanyName;
            user.BirthDate = currentUser.BirthDate;
            user.StreetAddress = currentUser.StreetAddress;
            user.StreetAddress2 = currentUser.StreetAddress2;
            user.ZipCode = currentUser.ZipCode;
            user.State = currentUser.State;
            user.Gender = currentUser.Gender;
           
            user.Rate = currentUser.Rate;

            userManager.Update(user);
            // Returns an HTTP 302 response to the browser, which causes the browser to make a GET request to the specified action, in this case the index action.
            return RedirectToAction("Index");

        }

        public FileContentResult PublicUserPhotos(string Id)
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindById(Id);

            return new FileContentResult(user.UserPhoto, "image/jpeg/jpg");

        }

        // GET: UserInfo/UserPublicProfile/
        [HttpGet]
        public ActionResult UserPublicProfile(string userEmail)
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByName(userEmail);
            return View("UserPublicProfile", user);
        }


        [Authorize]
        [HttpGet]
        public ActionResult ChangeUserPhotos(string Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser currentUser = _context.Users.Find(Id);
            if (currentUser == null)
            {
                return HttpNotFound();
            }
            return View(currentUser);

        }

        // POST: /ChangeUserPhotos/currentUser
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult ChangeUserPhotos ()
        {
            string userId = User.Identity.GetUserId();
            // To convert the user uploaded Photo as Byte Array before save to DB    

            byte[] imageData = null;

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase poImgFile = Request.Files["UserPhoto"];

                using (var binary = new BinaryReader(poImgFile.InputStream))
                {
                    imageData = binary.ReadBytes(poImgFile.ContentLength);
                }
            }

            var user = _context.Users.Find(userId);
            //Gets a value that indicates whether this instance received from the view is valid.
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            user.UserPhoto = null;

            user.UserPhoto = imageData;
            
            _context.SaveChanges();

          // Returns an HTTP 302 response to the browser, which causes the browser to make a GET request to the specified action, in this case the index action.

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(string Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser currentUser = _context.Users.Find(Id);
            if (currentUser == null)
            {
                return HttpNotFound();
            }
            return View(currentUser);

        }

        // POST: /UserInfo/Edit/abd34
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(ApplicationUser currentUser)
        {
            //Gets a value that indicates whether this instance received from the view is valid.
            if (!ModelState.IsValid)
            {
                return View(currentUser);

            }
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByIdAsync(User.Identity.GetUserId()).Result;

            user.FirstName = currentUser.FirstName;
            user.LastName = currentUser.LastName;
            user.SocialSecurity = currentUser.SocialSecurity;
            user.CompanyName = currentUser.CompanyName;
            user.BirthDate = currentUser.BirthDate;
            user.StreetAddress = currentUser.StreetAddress;
            user.StreetAddress2 = currentUser.StreetAddress2;
            user.ZipCode = currentUser.ZipCode;
            user.State = currentUser.State;
            user.Gender = currentUser.Gender;
            user.Rate = currentUser.Rate;
            userManager.Update(user);
            // Returns an HTTP 302 response to the browser, which causes the browser to make a GET request to the specified action, in this case the index action.
            return RedirectToAction("Index");
        }
    }
}
