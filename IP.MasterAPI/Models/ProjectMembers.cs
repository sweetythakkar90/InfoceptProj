using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.MasterAPI.Models
{
    public class ProjectMembers : GlobalModel
    {
        
        public int Id { get; set; }
        public int projId { get; set; }
        public int memberId { get; set; }
        public string memberName { get; set; }
        public int statusId { get; set; }
        public string statusName { get; set; }
    }
}
