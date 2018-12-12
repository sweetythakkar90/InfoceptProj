using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface IProjectMembersRepository
    {
        List<ProjectMembers> GetProjectMembersDetailsAsync(int ID, int projID);
        void InsertProjectMembersDetailsAsync(ProjectMembers projMembers);
        void UpdateProjectMembersDetailsAsync(ProjectMembers projMembers);
        void DeleteProjectMembersDetailsAsync(int ID);
        
        
    }
}
