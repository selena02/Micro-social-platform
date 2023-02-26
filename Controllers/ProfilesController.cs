using Micro_social_platform.Data;
using Micro_social_platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ExceptionServices;

namespace Micro_social_platform.Controllers
{
    [Authorize]
    public class ProfilesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ProfilesController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;

            if (User.IsInRole("User"))
            {
                ViewBag.AfisareButoane = true;
            }
            ViewBag.EsteAdmin = User.IsInRole("Admin");

            ViewBag.UserCurent = _userManager.GetUserId(User);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {

            var profiles = db.Profiles.Include("User");
            ViewBag.Profiles = profiles;
            var search = "";

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"].ToString();
            }

            
           
            int _perPage = 3;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Msg = TempData["message"].ToString();
            }

            int totalItems = profiles.Count();
            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);


            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }

          
            var paginatedProfiles = profiles.Skip(offset).Take(_perPage);


            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);

            ViewBag.Profiles = paginatedProfiles;
            if (search != "")
            {
                ViewBag.PaginationBaseUrl = "/Profiles/Index/?search=" + search + "&page";
            }
            else
            {
                ViewBag.PaginationBaseUrl = "/Profiles/Index/?page";
            }

            return View();
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult New(int id)
        {
            string userId = _userManager.GetUserId(User);
            int profileExists = db.Profiles.Where(m => m.UserId == userId).ToList().Count();
            if (profileExists > 0)
            {
                TempData["message"] = "You already have a profile";
                return RedirectToAction("Index","Home");
            }

            else
            {
                Profile profile = new Profile();
                return View(profile);
            }
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult New(Profile profile)
        {

            profile.UserId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                db.Profiles.Add(profile);
                db.SaveChanges();
                TempData["message"] = "Profile created";
                return RedirectToAction("Index","Home");
            }
            else
            {
               
                return View(profile);
            }
        }
        public IActionResult Show(string id)
        {
            var check = db.Profiles.Where(prof => prof.UserId == id);
            var are_friends = db.Friends.Where(friend => friend.User_sender_id == id && friend.User_receiver_id == _userManager.GetUserId(User) && friend.Has_accepted == true || friend.User_receiver_id == id && friend.User_sender_id == _userManager.GetUserId(User) && friend.Has_accepted == true);
            ViewBag.AreFriends = false;
            if (are_friends.Any()) 
            {
                ViewBag.AreFriends = true;
            }
            if (check.Any())
            {
                Profile profile = db.Profiles.Include("User")
                                         .Where(prof => prof.UserId == id)
                                         .First();
                ViewBag.AreProfil = true;
                ViewBag.Profile = profile;
                if (TempData.ContainsKey("message"))
                {
                    ViewBag.Msg = TempData["message"].ToString();
                }
                SetAccessRights();
                if (profile != null)
                {
                    return View(profile);
                }
            }
            ViewBag.AreProfil = false;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Msg = TempData["message"].ToString();
            }
          
                return View();
          
        }
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Delete(int id)
        {
            Profile profile = db.Profiles.Find(id);
            if (profile.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Profiles.Remove(profile);
                db.SaveChanges();
                TempData["message"] = "Profile deleted sucesfully";
                return RedirectToAction("Index", "Home");
            }
            else
            {

                TempData["message"] = "Profile cannot be deleted";
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id)
        {

            Profile profile = db.Profiles.Where(prof => prof.ProfileId == id)
                                         .First();
            ViewBag.Profile = profile;
            if (profile.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(profile);
            }

            else
            {
                TempData["message"] = "Profile cannot be edited";
                return RedirectToAction("Index","Home");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id, Profile requestProfile)
        {
            requestProfile.ProfileId = id;
            Profile profile = db.Profiles.Find(id);

            if (ModelState.IsValid)

            {
                if (profile.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    profile.FirstName = requestProfile.FirstName;
                    profile.LastName = requestProfile.LastName;
                    profile.ProfilePicture = requestProfile.ProfilePicture;
                    profile.Description = requestProfile.Description;
                    profile.IsPrivate = requestProfile.IsPrivate;
                    profile.JoinDate = requestProfile.JoinDate;
                    db.SaveChanges();
                    TempData["message"] = "Profile edited sucesfully";
                    return RedirectToAction("Index","Home");
                }

                else
                {
                    TempData["message"] = "Profile cannot be edited";
                    return RedirectToAction("Index","Home");
                }
            }
            else
                return View(requestProfile);
        }
    }
}
