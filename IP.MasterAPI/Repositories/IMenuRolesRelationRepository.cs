using IP.MasterAPI.Models;
using System.Collections.Generic;

namespace IP.MasterAPI.Repositories
{
    public interface IMenuRolesRelationRepository
    {
        List<MenuRolesRelation> GetMenuRolesRelationDetailsAsync(int ID, int roleId);
        void InsertMenuRolesRelationDetailsAsync(MenuRolesRelation relation);
        void UpdateMenuRolesRelationDetailsAsync(MenuRolesRelation relation);
        void DeleteMenuRolesRelationDetailsAsync(int Id);

    }
}
