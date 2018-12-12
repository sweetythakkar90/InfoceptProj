using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface ISubContractorRepository
    {
        List<SubContractor> GetSubContractorDetailsAsync(int subContractorID, char mode);

        void InsertSubContractorDetailsAsync(SubContractor sc, List<SubContractorSkillSetMapping> scSkillSets);

        List<SubContractor> UpdateSubContractorDetailsAsync(SubContractor sc, List<SubContractorSkillSetMapping> scSkillSets);

        List<SubContractor> DeleteSubContractorDetailsAsync(int subContractorID);

        List<SubContractor> DeleteSubContractorSkillSetsDetailsAsync(int subContractorkillSetMappingID);


    }
}
