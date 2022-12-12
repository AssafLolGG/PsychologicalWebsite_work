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
        public int last_page { get; set; }
    }
    public class ArticlesController : Controller
    {
        private static List<T> get_sublist<T>(List<T> input_list,int start_index,int end_index, int count)
        {
            // Create a new list to hold the sublist
            var sublist = new List<T>();
            int counter = 0;
            // Loop through the input list and add each item
            // to the sublist if its index is between the
            // start and end indices (inclusive)
            for(int i = 0;i < input_list.Count && counter != 10;i++)
            {
                if(i >= start_index && i <= end_index)
                {
                    sublist.Add(input_list[i]);
                    counter++;
                }
            }

            // Return the sublist
            return sublist;
        }

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
            mymodel.current_page = id;
            List<Article> articlesList = DatabaseAPI.getAllArticles();
            mymodel.last_page = ((DatabaseAPI.getAllArticles().Count() - 1) / 10) + 1;
            if(id <= mymodel.last_page && id > 0)
            {
                mymodel.articles = get_sublist<Article> (articlesList,(id - 1)* 10,(id) * 10 - 1, 10);
                return View("index",mymodel);
            }
            else
            {
                if(id > mymodel.last_page)
                {
                    return RedirectToAction("page","Articles",new { id = mymodel.last_page });
                }
                return RedirectToAction("page","Articles",new { id = 1 });
            }
        }
    }

   
}