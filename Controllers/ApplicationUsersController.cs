using Micro_social_platform.Data;
using Micro_social_platform.Data.Migrations;
using Micro_social_platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Micro_social_platform.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ApplicationUsersController(
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
            var users = db.ApplicationUsers.OrderBy(a => a.UserName);
            var search = "";
            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {

                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim();
                List<string> usersIds = db.ApplicationUsers.Where(at => at.UserName.Contains(search))
                                                            .Select(a => a.Id)
                                                            .ToList();

                users = (IOrderedQueryable<ApplicationUser>)db.ApplicationUsers.Where(user => usersIds.Contains(user.Id));
            }
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            SetAccessRights();
            ViewBag.SearchString = search;
            int _perPage = 3;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            int totalItems = users.Count();
            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);
            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }
            var paginatedApplicationUsers = users.Skip(offset).Take(_perPage);

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);

            ViewBag.Users = paginatedApplicationUsers;


            if (search != "")
            {
                ViewBag.PaginationBaseUrl = "/ApplicationUsers/Index/?search=" + search + "&page";
            }
            else
            {
                ViewBag.PaginationBaseUrl = "/ApplicationUsers/Index/?page";
            }
            return View();
        }
    }
}
