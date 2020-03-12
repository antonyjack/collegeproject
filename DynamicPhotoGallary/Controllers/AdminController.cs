using DynamicPhotoGallary.Models;
using DynamicPhotoGallary.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DynamicPhotoGallary.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                using (var entities = new DB_WebgalleryEntities())
                {
                    if (entities.Admins.Any(x => x.UserName.Equals(model.Username, StringComparison.InvariantCultureIgnoreCase) &&
                        x.Password.Equals(model.Password)))
                    {
                        FormsAuthentication.SetAuthCookie(model.Username, false);
                        return RedirectToAction("gallery");
                    }                        
                    else
                    {
                        ModelState.AddModelError("", "Username or password incorrect.");
                    }                    
                }
            }
            return View("Index", model);
        }

        [Authorize]
        public ActionResult Gallery()
        {
            using (var entities = new DB_WebgalleryEntities())
            {
                var model = entities.MainImages.Where(x => x.IsActive).ToList();
                return View(model);
            }            
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "home");
        }

        [Authorize]
        public ActionResult Edit(int image)
        {
            if (image <= 0)
                return HttpNotFound();

            using (var entities = new DB_WebgalleryEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                var model = entities.MainImages.Where(x => x.Id == image).FirstOrDefault();
                entities.Entry(model).Reference(x => x.Customer).Load();
                entities.Entry(model).Collection(x => x.SubImages).Load();
                return View(model);
            }            
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(MainImage model, HttpPostedFileBase file)
        {
            string FileExtension = (file.FileName.Split('.')[1]).ToLower();
            List<string> KnowExtensions = new List<string>() {
                "jpg", "jpeg", "png", "tif"
            };
            if (!KnowExtensions.Any(x => x == FileExtension))
                ModelState.AddModelError("file", "Invalid file format");

            if (ModelState.IsValid)
            {
                string filePath = Server.MapPath("~/images/") + model.Customer.Name.ToLower().Replace(" ", "_") + "/" + model.CustomerId + "/sub/";
                
                if (!System.IO.Directory.Exists(filePath))
                    System.IO.Directory.CreateDirectory(filePath);

                file.SaveAs(filePath + file.FileName);

                SubImage image = new SubImage()
                {
                    MainImageId = model.Id,
                    Name =  model.Customer.Name.ToLower().Replace(" ", "_") + "/" + model.CustomerId + "/sub/" + file.FileName
                };

                using (var entities = new DB_WebgalleryEntities())
                {
                    entities.SubImages.Add(image);
                    entities.SaveChanges();
                }

                return RedirectToAction("gallery");
            }
            return View(model);
        }
	}
}