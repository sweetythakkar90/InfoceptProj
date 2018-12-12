using IP.MasterAPI.Models;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace IP.MasterAPI.Repositories
{
    public interface IRolesRepository
    {
        List<Roles> GetRolesDetailsAsync(int ID);
        void InsertRolesDetailsAsync(Roles roles);
        List<Roles> UpdateRolesDetailsAsync(Roles roles);
        List<Roles> DeleteRolesDetailsAsync(int ID);

    }
}
