using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IP.MasterAPI.Models
{
    public class Menu : GlobalModel
    {
        public int Id { get; set; }
        [Required]
        public string menuName { get; set; }
        [Required]
        public int parentId { get; set; }
        public string parentMenu { get; set; }
        public int roleId { get; set; }
        public string roles { get; set; }
        public string controllerName { get; set; }
        public string actionName { get; set; }
        public string userRights { get; set; }

    }
}