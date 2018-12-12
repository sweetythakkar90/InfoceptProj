using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface ILoginDetailsRepository
    {
        List<LoginDetails> GetLoginDetailsAsync(int LoginID);
        void InsertLoginDetailsAsync(LoginDetails Login);
        List<LoginDetails> UpdateLoginDetailsAsync(LoginDetails Login);
        List<LoginDetails> DeleteLoginDetailsAsync(int LoginID);
        
    }
}
