using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface ISubSORTypeRepository
    {
        List<SubSORType> GetSubSORTypeDetailsAsync(int ID);

        void InsertSubSORTypeDetailsAsync(SubSORType sORType);

        List<SubSORType> UpdateSubSORTypeDetailsAsync(SubSORType sORType);

        List<SubSORType> DeleteSubSORTypeDetailsAsync(int ID);

    }
}
