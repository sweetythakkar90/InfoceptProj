using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IP.MasterAPI.Models
{
    public class StatusType : GlobalModel
    {

        [Key]
        public int ID { get; set; }
        [Required]
        public string name { get; set; }
  

    }
}