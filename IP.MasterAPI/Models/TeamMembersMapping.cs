using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP.MasterAPI.Models
{
    public class TeamMembersMapping : GlobalModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int teamID { get; set; }
        [Required]
        public int memberId{ get; set; }

        public string memberName { get; set; }
    }
}