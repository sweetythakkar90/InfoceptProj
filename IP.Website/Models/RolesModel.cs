using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IP.Website.Models
{
    public class RolesModel : GlobalModel
    {
        public int Id { get; set; }
        public string rolesName { get; set; }

        public List<MenuRolesRelationModel> menuRolesList { get; set; }
    }
}