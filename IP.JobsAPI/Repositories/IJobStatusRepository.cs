using IP.JobsAPI.Models;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace IP.JobsAPI.Repositories
{
    public interface IJobStatusRepository
    {
        List<JobStatus> GetJobStatusDetailsAsync(int ID);

        void InsertJobStatusDetailsAsync(JobStatus jStatus);

        List<JobStatus> UpdateJobStatusDetailsAsync(JobStatus jStatus);

        List<JobStatus> DeleteJobStatusDetailsAsync(int ID);

    }
}
