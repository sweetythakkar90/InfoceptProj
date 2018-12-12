using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IP.MasterAPI.Models
{
    public class MenuRolesRelation : GlobalModel
    {
        public int Id { get; set; }
        public int menuId { get; set; }
        [Required]
        public string menuName { get; set; }
        [Required]
        public int parentId { get; set; }
        [Required]
        public string parentMenuName { get; set; }

        public int roleId { get; set; }
        public string roles { get; set; }
        public string userRights { get; set; }

    }
}