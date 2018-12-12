using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.MasterAPI.Models
{
    public class ProjectJobTypes : GlobalModel
    {
        
        public int Id { get; set; }
        public int projId { get; set; }
        public string description { get; set; }
        

    }
}
