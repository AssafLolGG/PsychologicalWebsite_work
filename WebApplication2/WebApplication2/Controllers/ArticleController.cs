using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ViewModelOneArticle
    {
        public Article article { get; set; }
        public IEnumerable<Comment> comments { get; set; }
    }
    public class ArticleController : Controller
    {
        // GET: Article
        static IList<Article> articlesList;
        static IList<Comment> comments_of_article;
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        public ActionResult Index(int id=0)
        {
            if(DatabaseAPI.isArticleIdInArticles(id))
            {
                if(Session["lang"] == null)
                {
                    Session["lang"] = "en";
                }
                Session["image_path"] = "";
                articlesList = DatabaseAPI.getAllArticles();
                if(Session["CurrentArticleID"] == null)
                {
                    Session["CurrentArticleID"] = id;
                }

                comments_of_article = DatabaseAPI.GetAllCommentsOfArticle(id); // getting all comments

                int index_of_article = 0;
                for(int i = 0;i < articlesList.Count;i++)
                {
                    if(articlesList[i].Id == id)
                    {
                        index_of_article = i;
                    }
                }
                ViewModelOneArticle s = new ViewModelOneArticle();
                s.article = articlesList[index_of_article];
                s.comments = comments_of_article;
                return View(s);
            }
            else
            {
                return RedirectToAction("index","Articles");
            }
        }
        [HttpPost]
        public ActionResult reply(string reply_of_owner, string comment_id)
        {
            articlesList = DatabaseAPI.getAllArticles();
            int index_of_article = 0;
            for(int i = 0;i < articlesList.Count;i++) // finding the index of the article to reply to
            {
                if(articlesList[i].Id == int.Parse(Session["CurrentArticleID"].ToString()))
                {
                    index_of_article = i;
                }
            }
            comments_of_article = DatabaseAPI.GetAllCommentsOfArticle(int.Parse(Session["CurrentArticleID"].ToString()));
            for(int i = 0;i < comments_of_article.Count;i++) // setting the reply
            {
                if(comments_of_article[i].comment_id == int.Parse(comment_id))
                {
                   comments_of_article[i].reply = reply_of_owner;
                   comments_of_article[i].if_replied = true;
                    MvcApplication.DBconn.RunNonQuerySQL("UPDATE Comments SET reply_text = '" + reply_of_owner.Replace("'","\'") + "' , is_replied = TRUE where CommentID=" + comment_id);
                }
            }


            ViewModelOneArticle s = new ViewModelOneArticle();
            s.article = articlesList[index_of_article];
            s.comments = comments_of_article;
            return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult comment(string commenter_name,string comment_text, string photoChoice)
        {
            if(DatabaseAPI.isArticleIdInArticles(int.Parse(Session["CurrentArticleID"].ToString())))
            {
                articlesList = DatabaseAPI.getAllArticles();
                int index_of_article = 0;
                for(int i = 0;i < articlesList.Count;i++)
                {
                    if(articlesList[i].Id == int.Parse(Session["CurrentArticleID"].ToString()))
                    {
                        index_of_article = i;
                    }
                }

                if(commenter_name != "" && comment_text != "")
                {
                    MvcApplication.DBconn.RunNonQuerySQL("INSERT INTO Comments(CommenterName, CommentBody,is_replied, reply_text, ArticleID, PFP_PIC_ID, TimeStampComment) VALUES('" + commenter_name.Replace("'","''") + "','" + comment_text.Replace("'","''") + "', FALSE, '', " + int.Parse(Session["CurrentArticleID"].ToString()) + ", '" + updatePfp(photoChoice).Replace("'","''") + "', " + DateTimeOffset.Now.ToUnixTimeSeconds().ToString().Replace("'","''") + ")");
                }
                comments_of_article = DatabaseAPI.GetAllCommentsOfArticle(int.Parse(Session["CurrentArticleID"].ToString()));
                ViewModelOneArticle s = new ViewModelOneArticle();
                s.article = articlesList[index_of_article];
                s.comments = comments_of_article;
                return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
            }
            else
            {
                return RedirectToAction("index","articles");
            }
        }

        public ActionResult comments()
        {
            
            return View();
        }

        
        public string updatePfp(string selectValue)
        {
            switch(selectValue)
            {
                case "man1":
                    return "../Content/pfpImages/pfpman1.png";
                case "man2":
                    return "../Content/pfpImages/pfpman2.png";
                case "woman1":
                    return "../Content/pfpImages/pfpwoman1.png";
                case "woman2":
                    return "../Content/pfpImages/pfpwoman2.png";
                default:
                    return "../Content/pfpImages/pfpman1.png";
            }
        }

        public ActionResult edit()
        {
            if(Session["is_admin"] != null && (bool)Session["is_admin"] == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("index","Articles");
            }
        }
        [HttpPost]
        public ActionResult edit_article(string article_title_english,string article_title_hebrew,string article_title_arabic, string subject,HttpPostedFileBase article_picture,HttpPostedFileBase article_body_english,HttpPostedFileBase article_body_hebrew,HttpPostedFileBase article_body_arabic)
        {
            if(Session["is_admin"] != null && (bool)Session["is_admin"] == true)
            {
                articlesList = DatabaseAPI.getAllArticles();
                int current_article_id = (int)Session["CurrentArticleID"];
                int index_of_article = 0;
                for(int i = 0;i < articlesList.Count;i++)
                {
                    if(articlesList[i].Id == current_article_id)
                    {
                        index_of_article = i;
                    }
                }
                Article current_article = articlesList[index_of_article];
                string article_picture_path = current_article.articleImage, article_body_english_path = current_article.body_english, article_body_hebrew_path = current_article.body_hebrew, article_body_arabic_path = current_article.body_arabic;

                if(subject == "")
                {
                    subject = current_article.articleSubject;
                }

                if(article_title_arabic == "")
                {
                    article_title_arabic = current_article.title_arabic;
                }
                if(article_title_hebrew == "")
                {
                    article_title_hebrew = current_article.title_hebrew;
                }

                if(article_title_english == "")
                {
                    article_title_english = current_article.title_english;
                }
                if(article_picture != null)
                {
                    string file_extention = article_picture.FileName.Substring(article_picture.FileName.LastIndexOf("."),article_picture.FileName.Length - article_picture.FileName.LastIndexOf("."));
                    article_picture_path = "../ArticlesFiles/" + current_article_id.ToString() + file_extention;
                    article_picture.SaveAs(Server.MapPath("~/ArticlesFiles/") + current_article_id.ToString() + file_extention);
                }

                if(article_body_english != null)
                {
                    article_body_english.SaveAs(Server.MapPath("~/ArticlesFiles/" + current_article_id + "-english.docx"));
                    article_body_english_path = "../ArticlesFiles/" + current_article_id + "-english.html";
                    AdminController.ConvertDocToHtml(Server.MapPath("~/ArticlesFiles/" + current_article_id + "-english.docx"),(Server.MapPath("~/ArticlesFiles/") + current_article_id + "-english.html"));


                    string s = System.IO.File.ReadAllText(Server.MapPath("~/ArticlesFiles/" + current_article_id + "-english.html"),System.Text.Encoding.GetEncoding("windows-1255"));
                    s = s.Replace("\"" + current_article_id + "-english.files","\"" + "../ArticlesFiles/".Replace("\\","/") + current_article_id + "-english.files");
                    System.IO.File.WriteAllText(Server.MapPath("~/ArticlesFiles/" + current_article_id + "-english.html"),s);
                }

                if(article_body_hebrew != null)
                {
                    article_body_hebrew.SaveAs(Server.MapPath("~/ArticlesFiles/" + current_article_id + "-hebrew.docx"));
                    article_body_hebrew_path = "../ArticlesFiles/" + current_article_id + "-hebrew.html";
                    AdminController.ConvertDocToHtml(Server.MapPath("~/ArticlesFiles/" + current_article_id + "-hebrew.docx"),(Server.MapPath("~/ArticlesFiles/") + current_article_id + "-hebrew.html"));


                    string s = System.IO.File.ReadAllText(Server.MapPath("~/ArticlesFiles/" + current_article_id + "-hebrew.html"),System.Text.Encoding.GetEncoding("windows-1255"));
                    s = s.Replace("\"" + current_article_id + "-hebrew.files","\"" + "../ArticlesFiles/".Replace("\\","/") + current_article_id + "-hebrew.files");
                    System.IO.File.WriteAllText(Server.MapPath("~/ArticlesFiles/" + current_article_id + "-hebrew.html"),s);
                }


                if(article_body_arabic != null)
                {
                    article_body_arabic.SaveAs(Server.MapPath("~/ArticlesFiles/" + current_article_id + "-arabic.docx"));
                    article_body_arabic_path = "../ArticlesFiles/" + current_article_id + "-arabic.html";

                    AdminController.ConvertDocToHtml(Server.MapPath("~/ArticlesFiles/" + current_article_id + "-arabic.docx"),(Server.MapPath("~/ArticlesFiles/") + current_article_id + "-arabic.html"));


                    string s = System.IO.File.ReadAllText(Server.MapPath("~/ArticlesFiles/" + current_article_id + "-arabic.html"),System.Text.Encoding.GetEncoding("windows-1255"));
                    s = s.Replace("\"" + current_article_id + "-arabic.files","\"" + "../ArticlesFiles/".Replace("\\","/") + current_article_id + "-arabic.files");
                    System.IO.File.WriteAllText(Server.MapPath("~/ArticlesFiles/" + current_article_id + "-arabic.html"),s);
                }

                MvcApplication.DBconn.RunNonQuerySQL("UPDATE Articles SET body_english='" + article_body_english_path.Replace("'", "''") + "', body_hebrew= '" + article_body_hebrew_path.Replace("'","''") + "', body_arabic ='" + article_body_arabic_path.Replace("'","''") + "', ArticleImage ='" + article_picture_path.Replace("'","''") + "', ArticleSubject='" + subject.Replace("'","''") + "', title_english ='" + article_title_english.Replace("'","''") + "', title_arabic ='" + article_title_arabic.Replace("'","''") + "', title_hebrew='" + article_title_hebrew.Replace("'","''") + "' where ArticleID = " + current_article_id);

                ViewModelOneArticle m = new ViewModelOneArticle();
                m.article = articlesList[index_of_article];
                m.comments = comments_of_article;
                return RedirectToAction("index","Article",new { id = current_article_id });
            }
            else
            {
                return RedirectToAction("index","Articles");
            }
        }

        public ActionResult deleteComment(int id)
        {
            if(Session["is_admin"] != null && (bool)Session["is_admin"] == true)
            {
                MvcApplication.DBconn.RunNonQuerySQL("Delete from Comments Where CommentID=" + id);
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                return RedirectToAction("index","Articles");
            }
        }

        public ActionResult deleteArticle(int id)
        {
            if(Session["is_admin"] != null && (bool)Session["is_admin"] == true)
            {
                MvcApplication.DBconn.RunNonQuerySQL("Delete from Articles Where ArticleID=" + id);
                foreach(Comment comment in DatabaseAPI.GetAllCommentsOfArticle(id))
                {
                    MvcApplication.DBconn.RunNonQuerySQL("Delete from Comments Where CommentID=" + id); // deletes all comments of the deleted article.
                }
                return RedirectToAction("index","Articles");
            }
            else
            {
                return RedirectToAction("index","Articles");
            }
        }
    }
}