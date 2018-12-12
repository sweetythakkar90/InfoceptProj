using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.Website.Models
{
    public class ProjectSubContractorsModel : GlobalModel
    {
        public int Id { get; set; }
        public int projId { get; set; }
        public int subcontractorId { get; set; }
        public string subconName { get; set; }
        public int statusId { get; set; }
        public string statusName { get; set; }

    }
}