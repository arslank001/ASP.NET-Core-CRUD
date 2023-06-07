using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CRUDCoreApplication.Data;
using CRUDCoreApplication.Models.ViewModel;
using CRUDCoreApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUDCoreApplication.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        private readonly ApplicationContext context;
        private readonly IHostingEnvironment environment;

        public ImageController(ApplicationContext context, IHostingEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upload(ImageCreateModel ImageModel)
        {
            if (ModelState.IsValid)
            {
                var BasePath = environment.WebRootPath;
                var FilePath = "Content/Images/" + ImageModel.ImagePath.FileName;
                var FullPath = Path.Combine(BasePath, FilePath);
                UploadFile(ImageModel.ImagePath, FullPath);
                var ImageData = new ImageModel()
                {
                    Name = ImageModel.Name,
                    ImagePath = FilePath
                };
                context.Add(ImageData);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(ImageModel);
            }
  
        }
        [AllowAnonymous]
        public void UploadFile(IFormFile file, string path)
        {
            FileStream Stream = new FileStream(path, FileMode.Create);
            file.CopyTo(Stream);
        }

        public IActionResult Index()
        {
            var ImagesList = context.Images.ToList();
            return View(ImagesList);
        }

        public IActionResult Delete(int id)
        {
            var Img = context.Images.SingleOrDefault(e => e.ImageID == id);
            context.Remove(Img);
            context.SaveChanges();
            TempData["DeleteMsg"] = "Image has been deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
