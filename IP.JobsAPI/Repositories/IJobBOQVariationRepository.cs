using IP.JobsAPI.Models;
using System.Collections.Generic;

namespace IP.JobsAPI.Repositories
{
    public interface IJobBOQVariationRepository
    {
        List<JobBOQVariation> GetJobBOQVariationDetailsAsync(int Id,int jobId);
        void InsertJobBOQVariationDetailsAsync(JobBOQVariation jobBOQVariation);
        void UpdateJobBOQVariationDetailsAsync(JobBOQVariation jobBOQVariation);
        void DeleteJobBOQVariationDetailsAsync(int Id);
         
    }
}
