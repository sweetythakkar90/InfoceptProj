using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IP.Website.Models
{
    public class SubSORTypeModel : GlobalModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string name { get; set; }

        public int SORTypeID { get; set; }

        public string SORTypeName { get; set; }

    }
}