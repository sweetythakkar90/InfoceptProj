using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface ISkillSetsRepository
    {
        List<SkillSets> GetSkillSetsDetailsAsync(int ID);

        void InsertSkillSetsDetailsAsync(SkillSets skillSets);

        List<SkillSets> UpdateSkillSetsDetailsAsync(SkillSets skillSets);

        List<SkillSets> DeleteSkillSetsDetailsAsync(int ID);

    }
}
