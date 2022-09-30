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

    }
}