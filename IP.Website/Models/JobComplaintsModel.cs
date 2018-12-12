using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.Website.Models
{
    public class JobComplaintsModel : GlobalModel
    {
        public int Id { get; set; }
        public int jobID { get; set; }
        public string compliantDesc { get; set; }
        public int memberId { get; set; }
        public string memberName { get; set; }
        public int subcontractorID { get; set; }
        public string subConName { get; set; }
        public DateTime dueDate { get; set; }
        public string priority { get; set; }
    }
}
