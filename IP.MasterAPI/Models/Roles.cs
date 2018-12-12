using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IP.MasterAPI.Models
{
    public class Roles : GlobalModel
    {
        public int Id { get; set; }
        public string rolesName { get; set; }


        public List<MenuRolesRelation> menuRolesList { get; set; }
    }
}