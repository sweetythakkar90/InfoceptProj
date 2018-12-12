using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.MasterAPI.Models
{
    public class LoginDetails : GlobalModel
    {
        public int ID { get; set; }
        public int userId{ get; set; }
        public DateTime loginDate { get; set; }
        public Nullable<DateTime> logoutDate{ get; set; }
        

    }
}