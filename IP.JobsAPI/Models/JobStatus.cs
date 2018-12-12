using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IP.JobsAPI.Models
{
    public class JobStatus : GlobalModel
    {

        [Key]
        public int ID { get; set; }
        [Required]
        public string name { get; set; }
  

    }
}