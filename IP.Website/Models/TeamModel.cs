using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP.Website.Models
{
    public class TeamModel : GlobalModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string teamName { get; set; }
        public int statusId { get; set; }
        public string statusName { get; set; }
        public List<TeamMembersMappingModel> memberMapping { get; set; }


    }

}