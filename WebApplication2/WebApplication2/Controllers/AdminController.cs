using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using Word = Microsoft.Office.Interop.Word;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WebApplication2.Controllers
{
    public class AdminController:Controller
    {
        // GET: Admin
        
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(string username,string password)
        {
            if(username != "" && password != "")
            {
                if(MvcApplication.DBconn.RunDataSetSQL("Select * From Admins Where username='" + username.Replace("'","''") + "' and password='" + password.Replace("'","''") + "'").Tables[0].Rows.Count != 0)
                {
                    Session["is_admin"] = true;
                    return RedirectToAction("index", "home");
                }
            }
            
            return RedirectToAction("index");
        }

        public ActionResult add()
        {
            if(Session["is_admin"] != null && (bool)Session["is_admin"] == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("index","articles");
            }
        }
        public ActionResult add_course()
        {
            if(Session["is_admin"] != null && (bool)Session["is_admin"] == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("index","articles");
            }
        }

        public static void ConvertDocToHtml(object Sourcepath,object TargetPath)
    {

        Word._Application newApp = new Word.Application();
        Word.Documents d = newApp.Documents;
        object Unknown = Type.Missing;
        Word.Document od = d.Open(ref Sourcepath,ref Unknown,
                                 ref Unknown,ref Unknown,ref Unknown,
                                 ref Unknown,ref Unknown,ref Unknown,
                                 ref Unknown,ref Unknown,ref Unknown,
                                 ref Unknown,ref Unknown,ref Unknown,ref Unknown);
        object format = Word.WdSaveFormat.wdFormatHTML;



        newApp.ActiveDocument.SaveAs(ref TargetPath,ref format,
                    ref Unknown,ref Unknown,ref Unknown,
                    ref Unknown,ref Unknown,ref Unknown,
                    ref Unknown,ref Unknown,ref Unknown,
                    ref Unknown,ref Unknown,ref Unknown,
                    ref Unknown,ref Unknown);

        newApp.Documents.Close(Word.WdSaveOptions.wdDoNotSaveChanges);


    }
        [HttpPost]
        public ActionResult addCourse(HttpPostedFileBase sylabus,HttpPostedFileBase sylabus_hebrew,HttpPostedFileBase sylabus_russian,HttpPostedFileBase course_logo,string course_name,string course_name_hebrew, string course_name_russian, string course_price)
        {
            if(Session["is_admin"] != null && (bool)Session["is_admin"] == true)
            {
                try
                {
                    string path_of_image = "";
                    if(sylabus.ContentLength > 0 && course_logo.ContentLength > 0)
                    {
                        MvcApplication.DBconn.RunNonQuerySQL("INSERT INTO Courses(course_name, course_price, course_name_hebrew, course_name_russian) values('" + course_name.Replace("'","''") + "', '" + course_price.Replace("'", "''") + "', '" + course_name_hebrew.Replace("'","''") + "', '"+ course_name_russian.Replace("'","''") + "')"); // updating the db
                        List<Course> newCoursesList = DatabaseAPI.getAllCourses();
                        int new_id = newCoursesList[0].course_id;
                        sylabus.SaveAs(Server.MapPath("~/ArticlesFiles/Course" + new_id + ".docx"));
                        sylabus_hebrew.SaveAs(Server.MapPath("~/ArticlesFiles/CourseHebrew" + new_id + ".docx"));
                        sylabus_russian.SaveAs(Server.MapPath("~/ArticlesFiles/CourseRussian" + new_id + ".docx"));
                        string path_english = "~/ArticlesFiles/Course" + new_id.ToString() + ".docx";
                        string path_hebrew = "~/ArticlesFiles/CourseHebrew" + new_id.ToString() + ".docx";
                        string path_russian = "~/ArticlesFiles/CourseRussian" + new_id.ToString() + ".docx";
                        if(course_logo != null)
                        {
                            string file_extention = course_logo.FileName.Substring(course_logo.FileName.LastIndexOf("."),course_logo.FileName.Length - course_logo.FileName.LastIndexOf("."));
                            path_of_image = "../ArticlesFiles/Course" + new_id + file_extention;
                            course_logo.SaveAs(Server.MapPath("~/ArticlesFiles/Course") + new_id + file_extention);
                        }

                        MvcApplication.DBconn.RunNonQuerySQL("UPDATE Courses SET course_sylabus_file='" + path_english.Replace("'","''") + "', Course_logo_file ='" + path_of_image.Replace("'","''") + "', course_sylabus_file_hebrew='" + path_hebrew.Replace("'","''") + "', course_sylabus_file_russian='"+ path_russian +"' where course_id = " + new_id);

                        return RedirectToAction("index","Courses");
                    }

                    return RedirectToAction("add_course");
                }
                catch
                {
                    return RedirectToAction("add_course");
                }
                
            }
            return RedirectToAction("index","Courses");
        }
    [HttpPost]
        public ActionResult addFile(HttpPostedFileBase file,HttpPostedFileBase file_hebrew,HttpPostedFileBase file_russian, string title,string title_hebrew,string title_russian,HttpPostedFileBase articleImage, string subject)
        {
            if(Session["is_admin"] != null && (bool)Session["is_admin"] == true)
            {
                try
                {
                    string path_of_image = "";
                    if(file.ContentLength > 0)
                    {
                        MvcApplication.DBconn.RunNonQuerySQL("INSERT INTO Articles(title_english, body_english, title_hebrew, title_russian) values('" + title.Replace("'","''") + "', ' ', '" + title_hebrew.Replace("'","''") + "', '" + title_russian.Replace("'","''") + "')"); // updating the db
                        List<Article> newArticlesList = DatabaseAPI.getAllArticles();
                        int new_id = newArticlesList[0].Id;

                        file.SaveAs(Server.MapPath("~/ArticlesFiles/Article" + new_id + ".docx"));
                        file_hebrew.SaveAs(Server.MapPath("~/ArticlesFiles/ArticleHebrew" + new_id + ".docx"));
                        file_russian.SaveAs(Server.MapPath("~/ArticlesFiles/ArticleRussian" + new_id + ".docx"));
                        string path_english = "../ArticlesFiles/Article" + new_id + ".html";
                        string path_hebrew = "../ArticlesFiles/ArticleHebrew" + new_id + ".html";
                        string path_russian = "../ArticlesFiles/ArticleRussian" + new_id + ".html";

                        if(articleImage != null)
                        {
                            string file_extention = articleImage.FileName.Substring(articleImage.FileName.LastIndexOf("."),articleImage.FileName.Length - articleImage.FileName.LastIndexOf("."));
                            path_of_image = "../ArticlesFiles/" + new_id + file_extention;
                            articleImage.SaveAs(Server.MapPath("~/ArticlesFiles/") + new_id + file_extention);
                        }

                        MvcApplication.DBconn.RunNonQuerySQL("UPDATE Articles SET body_english='" + path_english.Replace("'","''") + "',body_hebrew = '" + path_hebrew.Replace("'","''") + "', body_russian = '" + path_russian.Replace("'","''") + "', ArticleImage ='" + path_of_image.Replace("'","''") + "', ArticleSubject='" + subject.Replace("'","''") + "' where ArticleID = " + new_id);

                        ConvertDocToHtml(Server.MapPath("~/ArticlesFiles/Article" + new_id + ".docx"),Server.MapPath(path_english));
                        ConvertDocToHtml(Server.MapPath("~/ArticlesFiles/ArticleHebrew" + new_id + ".docx"),Server.MapPath(path_hebrew));
                        ConvertDocToHtml(Server.MapPath("~/ArticlesFiles/ArticleRussian" + new_id + ".docx"),Server.MapPath(path_russian));

                        string s = System.IO.File.ReadAllText(Server.MapPath(path_english),System.Text.Encoding.GetEncoding("windows-1255"));
                        s = s.Replace("\"" + new_id + ".files","\"" + "../ArticlesFiles/Article".Replace("\\","/") + new_id + ".files");
                        System.IO.File.WriteAllText(Server.MapPath(path_english),s);

                        s = System.IO.File.ReadAllText(Server.MapPath(path_hebrew),System.Text.Encoding.GetEncoding("windows-1255"));
                        s = s.Replace("\"" + new_id + ".files","\"" + "../ArticlesFiles/ArticleHebrew".Replace("\\","/") + new_id + ".files");
                        System.IO.File.WriteAllText(Server.MapPath(path_hebrew),s);

                        s = System.IO.File.ReadAllText(Server.MapPath(path_english),System.Text.Encoding.GetEncoding("windows-1255"));
                        s = s.Replace("\"" + new_id + ".files","\"" + "../ArticlesFiles/ArticleRussian".Replace("\\","/") + new_id + ".files");
                        System.IO.File.WriteAllText(Server.MapPath(path_english),s);

                        return RedirectToAction("index","articles");
                    }

                    return RedirectToAction("add");
                }
                catch
                {
                    return RedirectToAction("add");
                }
            }
            else
            {
                return RedirectToAction("index","articles");
            }
        }
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image,int width,int height)
        {
            var destRect = new Rectangle(0,0,width,height);
            var destImage = new Bitmap(width,height);

            destImage.SetResolution(image.HorizontalResolution,image.VerticalResolution);

            using(var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using(var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image,destRect,0,0,image.Width,image.Height,GraphicsUnit.Pixel,wrapMode);
                }
            }

            return destImage;
        }
    }
}