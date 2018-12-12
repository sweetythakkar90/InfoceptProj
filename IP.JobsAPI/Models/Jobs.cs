using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IP.MasterAPI.Models;
namespace IP.JobsAPI.Models
{
    public class Jobs :GlobalModel
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
        public List<JobAssignment> jobAssign { get; set; }
        public List<JobRates> jobRates { get; set; }
        public List<JobBOQVariation> jobBOQVariation { get; set; }
        public List<JobDocuments> jobDocuments { get; set; }
        public List<JobComplaints> jobComplaints  { get; set; }
        public List<JobDefects> jobDefects { get; set; }

        public List<Project> project { get; set; }
        public List<Team> team { get; set; }
        public List<SubContractor> subcontractor { get; set; }
        public List<JobStatus> jobstatus { get; set; }
        public List<ProjectJobTypes> projectjobtypes { get; set; }
        public List<ProjectRates> projectRates { get; set; }
        public List<Members> members { get; set; }
        public List<JobRates> jRates { get; set; }
    }
}