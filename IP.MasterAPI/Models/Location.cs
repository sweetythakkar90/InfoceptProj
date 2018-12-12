using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IP.MasterAPI.Models
{
    
    public class Location : GlobalModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string name { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string streetNo { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string street { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string suburb { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string state { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string pincode { get; set; }
     


    }
}