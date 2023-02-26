using Micro_social_platform.Data;
using Micro_social_platform.Data.Migrations;
using Micro_social_platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;

namespace Micro_social_platform.Controllers
{
    public class FriendsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public FriendsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles = "User,Admin")]
        public IActionResult Index()
        {
            var friends = db.Friends.Include("ApplicationUser")
                                    .Where(friend => friend.User_receiver_id == _userManager.GetUserId(User) && friend.Has_accepted == false);
            ViewBag.Afisare = true;
            ViewBag.Friends = friends;
            if (friends.Any() == false)
            {
                ViewBag.Afisare = false;
            }
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"].ToString();

            }

            return View();
        }
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult Add_Friend(string id)
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"].ToString();
            }
            var check = db.Friends.Where(friend => friend.User_sender_id == _userManager.GetUserId(User) && friend.User_receiver_id == id || friend.User_sender_id == id && friend.User_receiver_id == _userManager.GetUserId(User));
            if (check.Any() == false && _userManager.GetUserId(User)!=id) 
            {

                Friend friend = new Friend();
                friend.User_sender_id = _userManager.GetUserId(User);
                friend.User_receiver_id = id;
                friend.User_sender_name = _userManager.GetUserName(User);
                friend.FriendDate = DateTime.Now;
                friend.Has_accepted = false;
                db.Friends.Add(friend);
                db.SaveChanges();
                TempData["message"] = "Request sent sucesfully";
                return RedirectToAction("Index", "ApplicationUsers");
            }
            else
            {
                TempData["message"] = "Request already sent";
                return RedirectToAction("Index", "ApplicationUsers");
            }
        }
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult Remove_Friend(string id)
        {
            Friend remove = db.Friends.Where(friend => friend.User_sender_id == id && friend.User_receiver_id == _userManager.GetUserId(User) || friend.User_receiver_id == id && friend.User_sender_id == _userManager.GetUserId(User)&&friend.Has_accepted==true)
                                            .FirstOrDefault();

            if (remove != null)
            {
                db.Friends.Remove(remove);
                db.SaveChanges();
                TempData["message"] = "Friend successfully removed";
                return RedirectToAction("Index", "ApplicationUsers");
            }

            else
            {
                TempData["message"] = "You cannot remove a user who is not your friend from the friends list";
                return RedirectToAction("Index", "ApplicationUsers");
            }

        }
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult Accept (int id)
        {

            Friend friend = db.Friends.Where(friend => friend.Id==id)
                                       .FirstOrDefault();     
            friend.Has_accepted = true;
            db.SaveChanges();
            TempData["message"] = "Request accepted succesfully";
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public ActionResult Refuse(int id)
        {
            Friend friend = db.Friends.Where(friend => friend.Id == id && friend.Has_accepted==false)
                                       .FirstOrDefault();
            if (friend != null)
            {
                db.Friends.Remove(friend);
                db.SaveChanges();
                TempData["message"] = "Request refused succesfully";
            }
            else
            {
                TempData["message"] = "Request already accepted";

            }
            return RedirectToAction("Index");
        }
    }
}
