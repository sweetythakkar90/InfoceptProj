using IP.JobsAPI.Models;
using System.Collections.Generic;

namespace IP.JobsAPI.Repositories
{
    public interface IJobAssignmentRepository
    {
        List<JobAssignment> GetJobAssignmentDetailsAsync(int jobAssignId,int jobId);
        void InsertJobAssignmentDetailsAsync(JobAssignment jobAssign);
        void UpdateJobAssignmentDetailsAsync(JobAssignment jobAssign);
        void DeleteJobAssignmentDetailsAsync(int jobAssignId);
        
    }
}
