using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IP.Website.Models
{
    public class MenuRolesRelationModel : GlobalModel
    {
        public int Id { get; set; }
        public int menuId { get; set; }
        [Required]
        public string menuName { get; set; }
        public int parentId { get; set; }
        [Required]
        public string parentMenuName { get; set; }
        [Required]
        public int roleId { get; set; }
        public string roles { get; set; }
        public string userRights { get; set; }

        public virtual ICollection<MenuRolesRelationModel> Group { get; set; }
    }
}