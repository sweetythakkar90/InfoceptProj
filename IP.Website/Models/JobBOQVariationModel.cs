using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.Website.Models
{
    public class JobBOQVariationModel : GlobalModel
    {
        public int Id { get; set; }
        public int jobID { get; set; }
        public int jobRatesId { get; set; }
        public string SORCode { get; set; }
        public decimal BOQQuantity { get; set; }
      
    }
}
