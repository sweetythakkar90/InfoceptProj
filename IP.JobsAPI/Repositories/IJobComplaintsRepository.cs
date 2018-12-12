using IP.JobsAPI.Models;
using System.Collections.Generic;

namespace IP.JobsAPI.Repositories
{
    public interface IJobComplaintsRepository
    {
        List<JobComplaints> GetJobComplaintsDetailsAsync(int Id,int jobId);
        void InsertJobComplaintsDetailsAsync(JobComplaints jobComplaints);
        void UpdateJobComplaintsDetailsAsync(JobComplaints jobComplaints);
        void DeleteJobComplaintsDetailsAsync(int Id);
        
    }
}
