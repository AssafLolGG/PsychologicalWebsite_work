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
        public IEnumerable<Course> courses { get; set; }
    }
    public class CoursesController:Controller
    {
        public coursesModel model;
        public ActionResult index()
        {
            //model.courses = new List<Course> { new Course{course_title="a",course_text="a",course_cost= 3,path_of_logo="3"} ,new Course { course_title = "a",course_text = "a",course_cost = 3,path_of_logo = "3" } };
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
                string body = "You choose to get information about the course{" + course_id_num.ToString() + "}";





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