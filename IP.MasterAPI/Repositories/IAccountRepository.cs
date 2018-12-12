using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface IAccountRepository
    {
        Account GetAuthenticateUser(string userName,string password);
        List<Account> GetUserDetailsAsync(int Id, int roleId);
        void InsertUserDetailsAsync(Account acct);
        void UpdateUserDetailsAsync(Account acct);
        void DeleteUserDetailsAsync(int Id);

        List<Roles> GetRoles(int Id);

    }
}
