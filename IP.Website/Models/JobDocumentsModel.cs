using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.Website.Models
{
    public class JobDocumentsModel : GlobalModel
    {
        public int Id { get; set; }
        public int jobID { get; set; }
        public string documentPath { get; set; }
        public string documentType { get; set; }
      
    }
}
