using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.Website.Models
{
    public class ProjectModel : GlobalModel
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
        public List<ProjectRatesModel> projRates { get; set; }
        public List<ProjectMembersModel> projMembers { get; set; }
        public List<ProjectMembersModel> members { get; set; }
        public List<ProjectSubContractorsModel> projSubContractors { get; set; }
        public List<ProjectSubContractorsModel> subContractors { get; set; }
        public List<ProjectJobTypesModel> projJobTypes { get; set; }
        public List<LocationModel> location { get; set; }
        public List<ClientModel> client { get; set; }
        public List<SORTypeModel> sortype { get; set; }
        public List<SubSORTypeModel> subsortype { get; set; }
        public List<TeamModel> team { get; set; }
        public List<SubContractorModel> subcontractor { get; set; }
        public List<StatusTypeModel> statusType { get; set; }

    }
}