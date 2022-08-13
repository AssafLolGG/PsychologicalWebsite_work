using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using System.Net;
using System.Net.Mail;
namespace WebApplication2.Controllers
{
    public class ViewModel
    {
        public IEnumerable<Article> articles { get; set; }
        public int current_page { get; set; }
    }
    public class ArticlesController : Controller
    {
        static IList<Article> articlesList;
        // GET: Article

        public ActionResult Index()
        {
            if(Session["lang"] == null)
            {
                Session["lang"] = "en";
            }
            articlesList = DatabaseAPI.getAllArticles();
            ViewModel mymodel = new ViewModel();
            mymodel.articles = articlesList;
            mymodel.current_page = 1;

            return RedirectToAction("page","Articles", new {id =1 });
        }
        [ActionName("page")]
        public ActionResult onClickButton(int id)
        {
            ViewModel mymodel = new ViewModel();
            mymodel.articles = DatabaseAPI.getAllArticles();
            mymodel.current_page = id;
            if(((id * 6) - 5) <= mymodel.articles.Count())
            {
                return View("index",mymodel);
            }
            else
            {
                mymodel.current_page = 1;
                return View("index",mymodel);
            }
        }
    }

   
}