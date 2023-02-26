using Micro_social_platform.Data;
using Micro_social_platform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace Micro_social_platform.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ArticlesController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
           
            var articles = db.Articles.Include("User");
            // AFISARE PAGINATA

            // Alegem sa afisam 3 articole pe pagina
            int _perPage = 3;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }


            // Fiind un numar variabil de articole, verificam de fiecare data utilizand 
            // metoda Count()

            int totalItems = articles.Count();


            // Se preia pagina curenta din View-ul asociat
            // Numarul paginii este valoarea parametrului page din ruta
            // /Articles/Index?page=valoare

            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);

            // Pentru prima pagina offsetul o sa fie zero
            // Pentru pagina 2 o sa fie 3 
            // Asadar offsetul este egal cu numarul de articole care au fost deja afisate pe paginile anterioare
            var offset = 0;

            // Se calculeaza offsetul in functie de numarul paginii la care suntem
            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }

            // Se preiau articolele corespunzatoare pentru fiecare pagina la care ne aflam 
            // in functie de offset
            var paginatedArticles = articles.Skip(offset).Take(_perPage);


            // Preluam numarul ultimei pagini

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);

            // Trimitem articolele cu ajutorul unui ViewBag catre View-ul corespunzator
            ViewBag.Articles = paginatedArticles;
            /*
            ViewBag.Articles = articles;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Msg = TempData["message"].ToString();
            }
            SetAccessRights();
            */
            ViewBag.PaginationBaseUrl = "/Articles/Index/?page";
            return View();
        }
        [Authorize(Roles = "User,Admin")]
        public IActionResult Show (int id) 
        {
            Article article = db.Articles.Include("Comments").Include("User").Include("Comments.User")
                                         .Where(art => art.Id == id) 
                                         .First();  
            ViewBag.Article = article;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Msg = TempData["message"].ToString();
            }
            SetAccessRights();
            return View(article);
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

        [HttpPost]
        public IActionResult Show([FromForm] Comment comment)
        {
            comment.Date = DateTime.Now;
            comment.UserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect("/Articles/Show/" + comment.ArticleId);
            }

            else
            {
                Article art = db.Articles.Include("Comments").Include("User").Include("Comments.User")
                                         .Where(art => art.Id == comment.ArticleId)
                                         .First();
                SetAccessRights();

                return View(art);
            }
        }
        [Authorize(Roles = "User,Admin")]
        public IActionResult New()
        {
            Article article = new Article();
            return View(article);
        }

      
       [Authorize(Roles="User,Admin")]
       [HttpPost]
        public IActionResult New (Article article) 
        {
            article.Date = DateTime.Now;
            article.UserId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                TempData["message"] = "Post loaded";
                return RedirectToAction("Index");
            }
            else
            {
                return View(article);
            }
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id) 
        {

            Article article = db.Articles.Where(art => art.Id == id)
                                         .First();
            ViewBag.Article=article;    
            if(article.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin") )
            {
                return View(article);
            }
            else 
            {
                TempData["message"] = "Post cannot be edited";
              return  RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id, Article requestArticle)
        {
            Article article = db.Articles.Find(id);

            if (ModelState.IsValid)
            {
                if (article.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    article.Title = requestArticle.Title;
                    article.Content = requestArticle.Content;
                    article.Date = requestArticle.Date;
                    db.SaveChanges();
                    TempData["message"] = "Post edited sucesfully";
                    return RedirectToAction("Index");
                }
                else 
                {
                    TempData["message"] = "Post cannot be edited";
                    return RedirectToAction("Index");

                }
            }
            else
            {
                return View(requestArticle);
            }
        }
       // [HttpPost]
        [Authorize(Roles ="User,Admin")]
        public ActionResult Delete(int id)
        {
            Article article = db.Articles.Include("Comments")
                                         .Where(art => art.Id == id)
                                         .First();

            if (article.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Articles.Remove(article);
                db.SaveChanges();
                TempData["message"] = "Post deleted sucesfully";
                return RedirectToAction("Index");
            }
            else 
            {
                TempData["message"] = "Post cannot be deleted";
                return RedirectToAction("Index");

            }
        }


    }
}
