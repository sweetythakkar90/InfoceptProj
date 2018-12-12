using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.Website.Models
{
    public class ProjectJobTypesModel : GlobalModel
    {
        public int Id { get; set; }
        public int projId { get; set; }
        public string description { get; set; }

    }
}