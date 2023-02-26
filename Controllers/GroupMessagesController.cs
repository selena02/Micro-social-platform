using Micro_social_platform.Data;
using Micro_social_platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Micro_social_platform.Controllers
{
    [Authorize]
    public class GroupMessagesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public GroupMessagesController(
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
        [HttpPost]
        public IActionResult Delete(int id)
        {

            GroupMessage post = db.GroupMessages.Find(id);
            if (post.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.GroupMessages.Remove(post);
                db.SaveChanges();
                TempData["message"] = "Message deleted sucesfully";
                return Redirect("/Groups/Show/" + post.GroupId);
            }
            else 
            {
                TempData["message"] = "Post cannot be deleted";
                return RedirectToAction("Index", "Groups");
            }
        }
        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id)
        {
            
            GroupMessage post = db.GroupMessages.Find(id);
            if (post.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                ViewBag.GroupMessage = post;
                return View(post);
            }
            else 
            {
                TempData["message"] = "Comment cannot be edited";
                return RedirectToAction("Index", "Groups");

            }
        }
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id, GroupMessage requestGroupMessage)
        {
            requestGroupMessage.MessageId = id;
            GroupMessage message = db.GroupMessages.Where(com => com.MessageId == id)
                                         .First();
            if (ModelState.IsValid)
            {
                if (message.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    message.Content = requestGroupMessage.Content;
                    db.SaveChanges();
                    return Redirect("/Groups/Show/" + message.GroupId);
                }
                else
                {
                    TempData["message"] = "Message cannot be edited";
                    return RedirectToAction("Index", "Groups");
                }
            }
            else 
            {
                return View(requestGroupMessage);   
            }

        }
    }
}
