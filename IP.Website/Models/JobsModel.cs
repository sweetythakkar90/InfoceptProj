using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.Website.Models
{
    public class JobsModel : GlobalModel
    {
        public int Id { get; set; }
        public int projId { get; set; }
        public int projJobTypeId { get; set; }
        public string projName { get; set; }
        public string jobDesc { get; set; }
        public string jobTypeName { get; set; }
        public string locCode { get; set; }
        public DateTime completionDate { get; set; }
        public int jobStatusId { get; set; }
        public string jobStatusName { get; set; }
        public List<JobAssignmentModel> jobAssign { get; set; }
        public List<JobRatesModel> jobRates { get; set; }
        public List<JobBOQVariationModel> jobBOQVariation { get; set; }
        public List<JobDocumentsModel> jobDocuments { get; set; }
        public List<JobComplaintsModel> jobComplaints  { get; set; }
        public List<JobDefectsModel> jobDefects { get; set; }

        public List<ProjectModel> project { get; set; }
        public List<TeamModel> team { get; set; }
        public List<SubContractorModel> subcontractor { get; set; }
        public List<JobStatusModel> jobstatus { get; set; }
        public List<ProjectJobTypesModel> projectjobtypes { get; set; }
        public List<ProjectRatesModel> projectRates { get; set; }
        public List<MembersModel> members { get; set; }
    }
}