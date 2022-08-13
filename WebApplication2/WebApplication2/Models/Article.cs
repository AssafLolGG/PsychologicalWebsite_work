using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string title_english { get; set; }
        public string title_hebrew { get; set; }
        public string title_arabic { get; set; }
        public string body_english { get; set; }
        public string body_hebrew { get; set; }
        public string body_arabic { get; set; }

        public string articleImage { get; set; }
        public string articleSubject { get; set; }
        
    }
}