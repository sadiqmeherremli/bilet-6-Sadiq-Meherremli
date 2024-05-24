using Microsoft.AspNetCore.Mvc;
using Pigga.DAl;
using Pigga.Models;

namespace Pigga.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChefController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ChefController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult Index()
        {
            return View(_context.Chefs.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]


        public IActionResult Create(Chef chef)
        {

            if (!ModelState.IsValid)
            {
                return View();

            }
            string fileName = Guid.NewGuid() + chef.ImgFile.FileName;
            string path = _environment.WebRootPath + @"\Upload\";

            using (FileStream stream = new FileStream(path + fileName, FileMode.Create))
            {
                chef.ImgFile.CopyTo(stream);
            }
            chef.ImgUrls = fileName;
            _context.Chefs.Add(chef);
            _context.SaveChanges();


            return RedirectToAction("Index");

        }
        public IActionResult Update(int id)
        {

            Chef chef = _context.Chefs.FirstOrDefault(x => x.Id == id);

            if (chef == null)
            {
                return RedirectToAction("Index");
            }


            return View(chef);

        }

        [HttpPost]

        public IActionResult Update(Chef newchef)

        {
            Chef oldchef = _context.Chefs.FirstOrDefault(x => x.Id == newchef.Id);

            if (!ModelState.IsValid)
            {
                return View(oldchef);

            }
            if (newchef.ImgFile != null)
            {
                string fileName = Guid.NewGuid() + newchef.ImgFile.FileName;
                string path = _environment.WebRootPath + @"\Upload\";

                using (FileStream stream = new FileStream(path + fileName, FileMode.Create))
                {
                    newchef.ImgFile.CopyTo(stream);
                }
                oldchef.ImgUrls = fileName;
            }

            oldchef.Name = newchef.Name;
            oldchef.Description = newchef.Description;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Chef chef = _context.Chefs.FirstOrDefault(x => x.Id == id);
            if (chef == null) return NotFound();


            string imagePath = Path.Combine(_environment.WebRootPath, "Upload", chef.ImgUrls);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.Chefs.Remove(chef);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
