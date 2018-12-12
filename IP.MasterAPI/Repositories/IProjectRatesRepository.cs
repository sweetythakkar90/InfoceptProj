using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface IProjectRatesRepository
    {
        List<ProjectRates> GetProjectRatesDetailsAsync(int ID, int projID);
        void InsertProjectRatesDetailsAsync(ProjectRates projRates);
        void UpdateProjectRatesDetailsAsync(ProjectRates projRates);
        void DeleteProjectRatesDetailsAsync(int ID);
        
        
    }
}
