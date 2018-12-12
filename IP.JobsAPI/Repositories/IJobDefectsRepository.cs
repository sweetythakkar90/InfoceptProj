using IP.JobsAPI.Models;
using System.Collections.Generic;

namespace IP.JobsAPI.Repositories
{
    public interface IJobDefectsRepository
    {
        List<JobDefects> GetJobDefectsDetailsAsync(int Id,int jobId);
        void InsertJobDefectsDetailsAsync(JobDefects jobDefects);
        void UpdateJobDefectsDetailsAsync(JobDefects jobDefects);
        void DeleteJobDefectsDetailsAsync(int Id);
        
    }
}
