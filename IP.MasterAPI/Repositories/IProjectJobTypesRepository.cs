using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface IProjectJobTypesRepository
    {
        List<ProjectJobTypes> GetProjectJobTypesDetailsAsync(int ID, int projID);
        void InsertProjectJobTypesDetailsAsync(ProjectJobTypes projJobTypes);
        void UpdateProjectJobTypesDetailsAsync(ProjectJobTypes projJobTypes);
        void DeleteProjectJobTypesDetailsAsync(int ID);
        
        
    }
}
