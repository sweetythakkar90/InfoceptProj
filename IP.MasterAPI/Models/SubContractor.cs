using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.MasterAPI.Models
{
    public class SubContractor : GlobalModel
    {
       
        public int Id { get; set; }
    
        public string subconName { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string phoneNumber { get; set; }
        public int statusId { get; set; }
        public string statusName { get; set; }
        public List<SubContractorSkillSetMapping> skillSelect { get; set; }

    }
}