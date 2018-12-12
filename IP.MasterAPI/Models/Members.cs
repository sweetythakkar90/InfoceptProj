using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP.MasterAPI.Models
{
    public class Members : GlobalModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [EmailAddress]
        public string email { get; set; }
        [Phone]
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public bool isTeamLead { get; set; }
        public Nullable<DateTime> lastLoginDate { get; set; }
        public int statusId { get; set; }
        public string statusName { get; set; }
        public List<MembersSkillSetMapping> skillSelect { get; set; }
    }
}