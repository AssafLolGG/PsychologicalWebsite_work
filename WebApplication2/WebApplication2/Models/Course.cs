using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    //logo, title, how much is it cost, text of the course
    public class Course
    {
        public int course_id { get; set; }
        public string course_cost { get; set; }
        public string path_of_logo { get; set; }
        public string course_file { get; set; }
        public string course_title { get; set; }
       
        
    }
}