using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP.Website.Models
{
    public class MembersSkillSetMappingModel : GlobalModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int memberId { get; set; }
        [Required]
        public int skillSetId { get; set; }

        public string skillSetName { get; set; }

    }

}