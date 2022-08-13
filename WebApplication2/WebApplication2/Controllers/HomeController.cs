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

    public class ModelContact
    {
        bool is_requested;
        bool is_failed_to_add_contact;
    }
    public class HomeController:Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public ActionResult hit_israel()
        {
            Session["lang"] = "he";
            return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult hit_english()
        {
            Session["lang"] = "en";
            return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult hit_arabic()
        {
            Session["lang"] = "ar";
            return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
        }
        public ActionResult Contact()
        {
            

            return View();
        }

        public ActionResult SendRequestContact(string contacter_name,string contacter_email,string contact_body)
        {

            if(contacter_email != "" && contacter_name != "" && contact_body != "")
            {
                const string subject = "New Request!";
                string body = "Name: " + contacter_name + "\nEmail: " + contacter_email + "\nBODY: " + contact_body;





                string username = "asafka73@gmail.com"; // high lvl sec 
                string password = "boodnletuoiyhpsk";
                ICredentialsByHost credentials = new NetworkCredential(username,password);

                SmtpClient smtpClient = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Credentials = credentials
                };

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(username);
                mail.To.Add(username);
                mail.Subject = subject;
                mail.Body = body;

                smtpClient.Send(mail);
                return RedirectToAction("index","Articles");
            }
            else
            {
                return RedirectToAction("Contact","Home");
            }
        }
    }
}