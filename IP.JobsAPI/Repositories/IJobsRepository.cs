using IP.JobsAPI.Models;
using System.Collections.Generic;

namespace IP.JobsAPI.Repositories
{
    public interface IJobsRepository
    {
        List<Jobs> GetJobsDetailsAsync(int jobsID,int projId);
        void InsertJobsDetailsAsync(Jobs Jobs);
        List<Jobs> UpdateJobsDetailsAsync(Jobs Jobs);
        void DeleteJobsDetailsAsync(int jobsID);
        
    }
}
