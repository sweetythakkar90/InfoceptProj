using System;
using System.ComponentModel.DataAnnotations;

namespace IP.MasterAPI.Models
{
    public class Company : GlobalModel
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