using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.MasterAPI.Models
{
    public class ProjectSubContractors : GlobalModel
    {
        
        public int Id { get; set; }
        public int projId { get; set; }
        public int subcontractorId { get; set; }
        public string subconName { get; set; }
        public int statusId { get; set; }
        public string statusName { get; set; }

        

    }
}
