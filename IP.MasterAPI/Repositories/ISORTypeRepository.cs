using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface ISORTypeRepository
    {
        List<SORType> GetSORTypeDetailsAsync(int ID);

        void InsertSORTypeDetailsAsync(SORType sorType);

        List<SORType> UpdateSORTypeDetailsAsync(SORType sorType);

        List<SORType> DeleteSORTypeDetailsAsync(int ID);

    }
}
