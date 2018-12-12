using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP.MasterAPI.Models
{
    public class SubContractorSkillSetMapping : GlobalModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int subcontractorID { get; set; }
        [Required]
        public int skillSetId { get; set; }
        public string skillSetName { get; set; }
    }
}