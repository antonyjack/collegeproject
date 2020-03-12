using DynamicPhotoGallary.Models;
using DynamicPhotoGallary.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynamicPhotoGallary.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<MainImageViewModel> images = new List<MainImageViewModel>();
            using (var entities = new DB_WebgalleryEntities())
            {
                images = entities.MainImages.Select(x => new MainImageViewModel()
                {
                    Image = x,
                    SubImageCount = x.SubImages.Count
                }).ToList();
            }
            return View(images);
        }

        public ActionResult About()
        {            
            return View();
        }

        public ActionResult Contact()
        {
            var model = new ContactModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Contact(ContactModel model, HttpPostedFileBase file)
        {
            if (string.IsNullOrEmpty(model.Pack))
                ModelState.AddModelError("Pack", "Pack is required.");
            if (string.IsNullOrEmpty(model.Size))
                ModelState.AddModelError("Size", "Size is required.");
            if (file == null)
                ModelState.AddModelError("file", "File is required.");

            string FileExtension = (file.FileName.Split('.')[1]).ToLower();
            List<string> KnowExtensions = new List<string>() {
                "jpg", "jpeg", "png", "tif"
            };
            if (!KnowExtensions.Any(x => x == FileExtension))
                ModelState.AddModelError("file", "Invalid file format");

            if (ModelState.IsValid)
            {
                Customer customer = new Customer()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Message = model.Message,
                    Phoneno = model.PhoneNo.ToString(),
                    Size = model.Size,
                    Pack = model.Pack,
                    Attach = file.FileName
                };

                using(var entities = new DB_WebgalleryEntities())
                {
                    entities.Customers.Add(customer);
                    entities.SaveChanges();
                }

                string filePath = Server.MapPath("~/images/") + customer.Name.ToLower().Replace(" ", "_") + "/" + customer.Id + "/";
                
                if (!System.IO.Directory.Exists(filePath))
                    System.IO.Directory.CreateDirectory(filePath);

                file.SaveAs(filePath + file.FileName);

                MainImage mainImage = new MainImage()
                {
                    IsActive = true,
                    Name = customer.Name.ToLower().Replace(" ", "_") + "/" + customer.Id + "/" + file.FileName,
                    CustomerId = customer.Id
                };
                using (var entities = new DB_WebgalleryEntities())
                {
                    entities.MainImages.Add(mainImage);
                    entities.SaveChanges();
                }

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        public ActionResult Pricing()
        {
            return View();
        }

        public ActionResult Picture(int id)
        {
            if (id <= 0)
                return HttpNotFound();

            using (var entities = new DB_WebgalleryEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                var model = entities.MainImages.Where(x => x.Id == id).FirstOrDefault();
                entities.Entry(model).Collection(x => x.SubImages).Load();
                return View(model);
            }
        }
    }
}