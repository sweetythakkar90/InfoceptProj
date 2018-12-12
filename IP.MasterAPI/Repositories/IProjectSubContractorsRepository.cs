using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface IProjectSubContractorsRepository
    {
        List<ProjectSubContractors> GetProjectSubContractorsDetailsAsync(int ID, int projID);
        void InsertProjectSubContractorsDetailsAsync(ProjectSubContractors projSubContractors);
        void UpdateProjectSubContractorsDetailsAsync(ProjectSubContractors projSubContractors);
        void DeleteProjectSubContractorsDetailsAsync(int ID);
        
        
    }
}
