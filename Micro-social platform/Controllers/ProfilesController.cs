using Micro_social_platform.Data;
using Micro_social_platform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ExceptionServices;

namespace Micro_social_platform.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly ApplicationDbContext db;
        public ProfilesController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            var profiles = db.Profiles;
            ViewBag.Profiles = profiles;
            return View();
        }
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(Profile profile)
        {

            try
            {
                db.Profiles.Add(profile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception )
            {
                RedirectToAction("New");
            }
            return View();
        }
      
        public IActionResult Show(int id)
        {
            Profile profile = db.Profiles.Where(prof => prof.ProfileId == id)
                                         .First();
            ViewBag.Profile = profile ;
            return View();
        }
    }
}
