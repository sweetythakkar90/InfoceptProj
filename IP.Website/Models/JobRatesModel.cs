using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.Website.Models
{
    public class JobRatesModel : GlobalModel
    {
        public int Id { get; set; }
        public int jobID { get; set; }
        public int projRatesId { get; set; }
        public string SORCode { get; set; }
        public decimal propsedBOQQuantity { get; set; }
        public decimal actualBOQQuantity { get; set; }
    }
}
