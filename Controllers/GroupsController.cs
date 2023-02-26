using Micro_social_platform.Data;
using Micro_social_platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Micro_social_platform.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public GroupsController(
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

        [Authorize(Roles = "User,Admin")]
        public IActionResult Index()
        {
            var groups = db.Groups.Include("User");
           // ViewBag.Groups = groups;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"].ToString();

            }
            int _perPage = 3;
            SetAccessRights();
            int totalItems = groups.Count();

            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);

           
            var offset = 0;

            
            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }

         
            var paginatedArticles = groups.Skip(offset).Take(_perPage);
           

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);
          
            ViewBag.Groups = paginatedArticles;
            ViewBag.PaginationBaseUrl = "/Groups/Index/?page";
            return View();
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Show(int id)
        {
            var check = db.UserGroups.Where(grup => grup.GroupId == id && grup.UserId == _userManager.GetUserId(User));
            if (check.Any() || User.IsInRole("Admin"))
            {
                Group group = db.Groups.Include("GroupMessages").Include("User").Include("GroupMessages.User")
                                             .Where(art => art.GroupId == id)
                                             .First();
                if (TempData.ContainsKey("message"))
                {
                    ViewBag.Msg = TempData["message"].ToString();
                }
                ViewBag.Group = group;
                SetAccessRights();
                return View(group);
            }
            else
            {
                TempData["message"] = "Join group in order to see group messages";
                return RedirectToAction("Index");

            }
        }
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Show([FromForm] GroupMessage message)
        {
            message.Date = DateTime.Now;
            message.UserId = _userManager.GetUserId(User);
            var check = db.UserGroups.Where(grup => grup.GroupId == message.GroupId && grup.UserId == _userManager.GetUserId(User));
            if (check.Any() || User.IsInRole("Admin"))
            {
                if (ModelState.IsValid)
                {
                    db.GroupMessages.Add(message);
                    db.SaveChanges();
                    return Redirect("/Groups/Show/" + message.GroupId);
                }

                else
                {
                    Group group = db.Groups.Include("GroupMessages").Include("User").Include("GroupMessages.User")
                                   .Where(art => art.GroupId == message.GroupId)
                                   .First();
                    SetAccessRights();
                    return View(group);
                }
            }
            else
            {
                TempData["message"] = "Join group in order to send group messages";
                return RedirectToAction("Index");
            }

        }
        [Authorize(Roles = "Admin")]
        public IActionResult New()
        {
            Group group = new Group();
            return View(group);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult New(Group group)
        {
            SetAccessRights();
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                TempData["message"] = "Group created";
                return RedirectToAction("Index");
            }
            else
            {
                return View(group);
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            SetAccessRights();
            Group group = db.Groups.Where(art => art.GroupId == id)
                                         .First();
            ViewBag.Group = group;
            if (User.IsInRole("Admin"))
            {
                return View(group);
            }
            else
            {
                TempData["message"] = "Group cannot be edited";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, Group requestGroup)
        {
            requestGroup.GroupId = id;
            Group group = db.Groups.Where(art => art.GroupId == id)
                                   .First();
            SetAccessRights();

            if (ModelState.IsValid && User.IsInRole("Admin"))
            { 
                    group.Description = requestGroup.Description;
                    group.Name = requestGroup.Name;
                    db.SaveChanges();
                    TempData["message"] = "Group edited sucesfully";
                    return RedirectToAction("Index");
            }
            else
            {
                return View(requestGroup);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id) 
        {
             Group group=db.Groups.Include("GroupMessages")
                                  .Where(art => art.GroupId == id).First();
             SetAccessRights();
            if (User.IsInRole("Admin"))
            {
                db.Groups.Remove(group);
                db.SaveChanges();
                TempData["message"] = "Group deleted sucesfully";
                return RedirectToAction("Index");
            }
            else 
            {
                TempData["message"] = "Group cannot be deleted";
                return RedirectToAction("Index");
            }


        }
    }
}
