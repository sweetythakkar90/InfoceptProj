using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface ITeamRepository
    {
        List<Team> GetTeamDetailsAsync(int TeamID,char mode);

        void InsertTeamDetailsAsync(Team mem, List<TeamMembersMapping> memberMapping);

        List<Team> UpdateTeamDetailsAsync(Team mem, List<TeamMembersMapping> memberMapping);

        List<Team> DeleteTeamDetailsAsync(int TeamID);

        List<Team> DeleteTeamMemberMappingDetailsAsync(int ttMembersMappingID);


    }
}
