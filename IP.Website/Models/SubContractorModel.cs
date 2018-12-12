using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IP.Website.Models
{
    public class SubContractorModel : GlobalModel
    {
        public int Id { get; set; }

        public string subconName { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string phoneNumber { get; set; }
        public int statusId { get; set; }
        public string statusName { get; set; }
        public List<SubContractorSkillSetMappingModel> skillSelect { get; set; }
    }

}