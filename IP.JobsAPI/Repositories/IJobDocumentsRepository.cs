using IP.JobsAPI.Models;
using System.Collections.Generic;

namespace IP.JobsAPI.Repositories
{
    public interface IJobDocumentsRepository
    {
        List<JobDocuments> GetJobDocumentsDetailsAsync(int Id, int jobId);
        void InsertJobDocumentsDetailsAsync(JobDocuments jobDocuments);
        void UpdateJobDocumentsDetailsAsync(JobDocuments jobDocuments);
        void DeleteJobDocumentsDetailsAsync(int Id);
        
    }
}
