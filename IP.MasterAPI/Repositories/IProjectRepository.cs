using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface IProjectRepository
    {
        List<Project> GetProjectDetailsAsync(int ID,int clientID);
        void InsertProjectDetailsAsync(Project proj);
        List<Project> UpdateProjectDetailsAsync(Project proj);
        void DeleteProjectDetailsAsync(int ID);
        
        
    }
}
