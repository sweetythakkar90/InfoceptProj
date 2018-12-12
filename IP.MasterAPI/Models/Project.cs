using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.MasterAPI.Models
{
    public class Project : GlobalModel
    {
        public int Id { get; set; }
        public string projName { get; set; }
        public int locId { get; set; }
        public string locName { get; set; }
        public int clientId { get; set; }
        public string clientName { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int statusId { get; set; }
        public string statusName { get; set; }
        public List<ProjectRates> projRates { get; set; }
        public List<ProjectMembers> projMembers { get; set; }
        public List<ProjectMembers> members { get; set; }
        public List<ProjectSubContractors> projSubContractors { get; set; }
        public List<ProjectSubContractors> subContractors { get; set; }
        public List<ProjectJobTypes> projJobTypes { get; set; }
        public List<Location> location { get; set; }
        public List<Client> client { get; set; }
        public List<SORType> sortype { get; set; }
        public List<SubSORType> subsortype { get; set; }
        public List<Team> team { get; set; }
        public List<SubContractor> subcontractor { get; set; }
        public List<StatusType> statusType { get; set; }
    }
}