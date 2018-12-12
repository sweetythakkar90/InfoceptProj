using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.JobsAPI.Models
{
    public class JobDocuments : GlobalModel
    {
        public int Id { get; set; }
        public int jobID { get; set; }
        public string documentPath { get; set; }
        public string documentType { get; set; }
      
    }
}
