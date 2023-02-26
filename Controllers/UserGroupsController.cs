using Micro_social_platform.Data;
using Micro_social_platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Micro_social_platform.Controllers
{
    [Authorize]
    public class UserGroupsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserGroupsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            if (TempData.ContainsKey("UserGroupId"))
            {
                ViewBag.UserGroupId = TempData["UserGroupId"];
            }
            if (TempData.ContainsKey("message")) 
            {
                ViewBag.Msg = TempData["message"].ToString();
            }
            if (TempData.ContainsKey("UserGroupId2"))
            {
                ViewBag.UserGroupId2 = TempData["UserGroupId2"];
            }
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
         public ActionResult AddUserGroup(int id) 
        {
            var check = db.UserGroups.Where(grup => grup.GroupId == id && grup.UserId== _userManager.GetUserId(User));
            if (check.Any() == false)
            {
                UserGroup usergroup = new UserGroup();
                usergroup.UserId = _userManager.GetUserId(User);
                usergroup.GroupId = id;
                usergroup.GroupDate = DateTime.Now;
                db.UserGroups.Add(usergroup);
                db.SaveChanges();
                TempData["UserGroupId"] = id;
               return RedirectToAction("Index");
            }
            else 
            {
                TempData["message"] = "You are already part of this group";
                TempData["UserGroupId2"] = id;
                return RedirectToAction("Index");   
            }
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult RemoveUserGroup(int id) 
        {
            UserGroup remove = db.UserGroups.Where(grup => grup.GroupId == id && grup.UserId == _userManager.GetUserId(User))
                                            .FirstOrDefault();

            ViewBag.EsteAdmin = User.IsInRole("Admin");

            if (remove != null && remove.UserId == _userManager.GetUserId(User)) 
            {
                db.UserGroups.Remove(remove);
                db.SaveChanges();
                TempData["message"] = "Group left succesfully";
                return RedirectToAction("Index","Groups");
            }
            else 
            {
                TempData["message"] = "Can't take other users out, only yourself";
                return RedirectToAction("Index", "Groups");
            }

        }
        [Authorize(Roles = "Admin")]
        public IActionResult Show(int id) 
        {

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Msg = TempData["message"].ToString();
            }
            var usergroup = db.UserGroups.Include("ApplicationUser")
                                         .Where(grup => grup.GroupId == id);
            ViewBag.Users = usergroup;
            if (usergroup.Any() == false) 
            {
                ViewBag.NoMembers = true;
                return View();
            }
            return View();

        }
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveAdminGroup(int id) 
        {
            UserGroup remove = db.UserGroups.Where(grup => grup.Id == id)
                                            .FirstOrDefault();

            if (User.IsInRole("Admin") && remove != null) 
            {
                db.UserGroups.Remove(remove);
                db.SaveChanges();
                TempData["message"] = "Member removed succesfully";
                return RedirectToAction("Index", "Groups"); ;
            }
            else 
            {
                TempData["message"] = "Member was not removed";
                return RedirectToAction("Index", "Groups");
            }

        }


    }
}
