using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.Website.Models
{
    public class JobAssignmentModel : GlobalModel
    {
        public int Id { get; set; }
        public int jobID { get; set; }
        public int teamID { get; set; }
        public string teamName { get; set; }
        public int subcontractorID { get; set; }
        public string subConName { get; set; }
        public DateTime jobAssignDate { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string notes { get; set; }
      
    }
}
