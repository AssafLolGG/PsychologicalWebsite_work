using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Comment
    {
        public string comment_text { get; set; }
        public string commenter_name { get; set; }
        public int comment_id { get; set; }
        public bool if_replied { get; set; }
        public string reply { get; set; }

        public int articleID { get; set; }
        public string PFP_PIC_ID { get; set; }
        public long TimeStamp{get; set;} 
    }
}