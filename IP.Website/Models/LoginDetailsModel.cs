using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.Website.Models
{
    public class LoginDetailsModel : GlobalModel
    {
        public int ID { get; set; }
        public int userId{ get; set; }
        public DateTime loginDate { get; set; }
        public Nullable<DateTime> logoutDate{ get; set; }
        
        public string userType { get; set; }

    }
}