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
    public class coursesModel
    {
        public List<Course> courses { get; set; }
    }
    public class CoursesController:Controller
    {
        public coursesModel model;
        public ActionResult index()
        {
            model = new coursesModel();
            model.courses = DatabaseAPI.getAllCourses();
            return View(model);
        }

        public ActionResult MailCourses(string email_address,string course_id)
        {
            int course_id_num = -1;
            if(!int.TryParse(course_id, out course_id_num))
            {
                course_id_num = -1;
            }
            
            if(email_address != "" && course_id_num != -1)
            {
                const string subject = "New Request!";
                string body = "You choose to get information about the course \"" + DatabaseAPI.getCourseByID(course_id_num).course_title + "\".\nhere's is its sylabus";



                string ownerMail = "asafka73@gmail.com";

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
                mail.To.Add(email_address);
                mail.Subject = subject;
                mail.Body = body;

                System.Net.Mail.Attachment attachment;
                if(Session["lang"] != null && ((string)Session["lang"] == "he"))
                {
                    attachment = new System.Net.Mail.Attachment(Server.MapPath(DatabaseAPI.getCourseByID(course_id_num).course_file_hebrew));
                }
                else if(Session["lang"] != null && ((string)Session["lang"] == "ar"))
                {
                    attachment = new System.Net.Mail.Attachment(Server.MapPath(DatabaseAPI.getCourseByID(course_id_num).course_file_russian));
                }
                else
                {
                    attachment = new System.Net.Mail.Attachment(Server.MapPath(DatabaseAPI.getCourseByID(course_id_num).course_file));
                }
                mail.Attachments.Add(attachment);

                smtpClient.Send(mail);

                mail = new MailMessage();
                mail.From = new MailAddress(username);
                mail.To.Add(ownerMail);
                mail.Subject ="New interested in the course.";
                mail.Body = "his email is: " + email_address + ".\nthe course he is interested in is: " + DatabaseAPI.getCourseByID(course_id_num).course_title + ".";
                if(Session["lang"] != null && ((string)Session["lang"] == "he"))
                {
                    attachment = new System.Net.Mail.Attachment(Server.MapPath(DatabaseAPI.getCourseByID(course_id_num).course_file_hebrew));
                }
                else if(Session["lang"] != null && ((string)Session["lang"] == "ar"))
                {
                    attachment = new System.Net.Mail.Attachment(Server.MapPath(DatabaseAPI.getCourseByID(course_id_num).course_file_russian));
                }
                else
                {
                    attachment = new System.Net.Mail.Attachment(Server.MapPath(DatabaseAPI.getCourseByID(course_id_num).course_file));
                }
                mail.Attachments.Add(attachment);

                smtpClient.Send(mail);

                return RedirectToAction("index","Articles");
            }
            else
            {
                return RedirectToAction("index","Courses");
            }
        }

    }
}