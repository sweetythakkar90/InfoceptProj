using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.JobsAPI.Models
{
    public class JobBOQVariation : GlobalModel
    {
        public int Id { get; set; }
        public int jobID { get; set; }
        public int jobRatesId { get; set; }
        public string SORCode { get; set; }
        public decimal BOQQuantity { get; set; }
      
    }
}
