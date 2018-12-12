using IP.JobsAPI.Models;
using System.Collections.Generic;

namespace IP.JobsAPI.Repositories
{
    public interface IJobRatesRepository
    {
        List<JobRates> GetJobRatesDetailsAsync(int Id, int jobId);
        void InsertJobRatesDetailsAsync(JobRates jobRates);
        void UpdateJobRatesDetailsAsync(JobRates jobRates);
        void DeleteJobRatesDetailsAsync(int Id);
        
    }
}
