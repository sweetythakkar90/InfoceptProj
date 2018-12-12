using System;
using System.ComponentModel.DataAnnotations;

namespace IP.Website.Models
{
    public class CompanyModel : GlobalModel
    {
        [Key]
        public int companyID { get; set; }
        [Required]
        public string companyName { get; set; }
        [EmailAddress]
        public string email { get; set; }
        [Phone]
        public string phoneNumber { get; set; }
       
        public int statusID { get; set; }

    }
}