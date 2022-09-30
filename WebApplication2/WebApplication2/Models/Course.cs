using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    //logo, title, how much is it cost, text of the course
    public class Course
    {
        public string path_of_logo { get; set; }
        public string course_title { get; set; }
        public int course_cost { get; set; }
        public string course_text { get; set; }
    }
}