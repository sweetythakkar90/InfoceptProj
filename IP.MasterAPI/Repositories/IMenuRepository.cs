using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface IMenuRepository
    {
        List<Menu> GetMenuDetails(int roleId,char mode);


    }
}
