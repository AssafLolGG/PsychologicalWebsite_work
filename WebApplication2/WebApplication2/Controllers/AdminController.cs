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
        public ActionResult addFile(HttpPostedFileBase file, string title,HttpPostedFileBase articleImage, string subject)
        {
            if(Session["is_admin"] != null && (bool)Session["is_admin"] == true)
            {
                try
                {
                    string path_of_image = "";
                    if(file.ContentLength > 0)
                    {
                        MvcApplication.DBconn.RunNonQuerySQL("INSERT INTO Articles(title_english, body_english) values('" + title.Replace("'","''") + "', ' ')"); // updating the db
                        List<Article> newArticlesList = DatabaseAPI.getAllArticles();
                        int new_id = newArticlesList[0].Id;
                        file.SaveAs(Server.MapPath("~/ArticlesFiles/" + new_id + ".docx"));
                        string path = "../ArticlesFiles/" + new_id + ".html";
                        if(articleImage != null)
                        {
                            string file_extention = articleImage.FileName.Substring(articleImage.FileName.LastIndexOf("."),articleImage.FileName.Length - articleImage.FileName.LastIndexOf("."));
                            path_of_image = "../ArticlesFiles/" + new_id + file_extention;
                            articleImage.SaveAs(Server.MapPath("~/ArticlesFiles/") + new_id + file_extention);
                        }

                        MvcApplication.DBconn.RunNonQuerySQL("UPDATE Articles SET body_english='" + path.Replace("'","''") + "', ArticleImage ='" + path_of_image.Replace("'","''") + "', ArticleSubject='" + subject.Replace("'","''") + "' where ArticleID = " + new_id);

                        ConvertDocToHtml(Server.MapPath("~/ArticlesFiles/" + new_id + ".docx"),(Server.MapPath("~/ArticlesFiles/") + new_id + ".html"));


                        string s = System.IO.File.ReadAllText(Server.MapPath("~/ArticlesFiles/" + new_id + ".html"),System.Text.Encoding.GetEncoding("windows-1255"));
                        s = s.Replace("\"" + new_id + ".files","\"" + "../ArticlesFiles/".Replace("\\","/") + new_id + ".files");
                        System.IO.File.WriteAllText(Server.MapPath("~/ArticlesFiles/" + new_id + ".html"),s);

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