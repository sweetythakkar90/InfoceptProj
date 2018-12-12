using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IP.Website.Models
{
    public class LocationModel : GlobalModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string name { get; set; }
        public string streetNo { get; set; }
        public string street { get; set; }
        public string suburb { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
     


    }
}