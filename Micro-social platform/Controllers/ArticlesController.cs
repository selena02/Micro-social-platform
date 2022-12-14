using Micro_social_platform.Data;
using Micro_social_platform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Micro_social_platform.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext db;
        public ArticlesController (ApplicationDbContext context) 
        {
            db = context;
        }
        public IActionResult Index()
        {
            var articles = db.Articles;
            ViewBag.Articles = articles;
            return View();
        }
        public IActionResult Show (int id) 
        {
            Article article = db.Articles.Include("Comments")
                                         .Where(art => art.Id == id) 
                                         .First();  
            ViewBag.Article = article; 
            return View();
        }

        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        public IActionResult New(Article article) 
        {

            try 
            {
                db.Articles.Add(article);   
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception)
            {
                RedirectToAction("New");
            }
            return View();
        }

        public IActionResult Edit(int id) 
        {

            Article article = db.Articles.Where(art => art.Id == id)
                                         .First();
            ViewBag.Article=article;    
            return View();

        }

        [HttpPost]
        public IActionResult Edit(int id, Article requestArticle)
        {
            Article article = db.Articles.Find(id);
            try
            {
                {
                    article.Title = requestArticle.Title;
                    article.Content = requestArticle.Content;
                    article.Date = requestArticle.Date;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            catch (Exception)
            {
                return RedirectToAction("Edit", id);
            }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
