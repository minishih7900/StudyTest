using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Models.Models
{
    public class member
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    public class memberDetail : member
    {
        public string name { get; set; }
        public DateTime date { get; set; }
        public int age { get; set; }
    }
}