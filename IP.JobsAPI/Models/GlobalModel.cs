using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.JobsAPI.Models
{
    public class GlobalModel
    {
        public DateTime createdDate { get; set; }
        public Nullable<DateTime> modifiedDate { get; set; }
        public int userId {get;set;}
    }
}