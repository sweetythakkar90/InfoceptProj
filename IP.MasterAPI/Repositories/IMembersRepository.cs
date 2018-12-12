using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface IMembersRepository
    {
        List<Members> GetMembersDetailsAsync(int membersID,char mode);

        void InsertMembersDetailsAsync(Members mem, List<MembersSkillSetMapping> memberSkillSets);

        List<Members> UpdateMembersDetailsAsync(Members mem, List<MembersSkillSetMapping> memberSkillSets);

        List<Members> DeleteMembersDetailsAsync(int membersID);

        List<Members> DeleteMembersSkillSetsDetailsAsync(int memberSkillSetMappingID);


    }
}
