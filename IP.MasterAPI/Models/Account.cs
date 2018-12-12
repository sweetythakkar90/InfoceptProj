using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IP.MasterAPI.Models
{
    public class Account : GlobalModel
    {
        public int Id { get; set; }
        [Required]
        public int uId { get; set; }
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
        public int memberId { get; set; }
        public string memberName { get; set; }
        public int subcontractorId { get; set; }
        public string subcontractorName { get; set; }
        public int roleId { get; set; }
        public string roles { get; set; }
        public string userType { get; set; }
        public int statusId { get; set; }
        public string statusName { get; set; }
        public List<StatusType> statusTypeList { get; set; }
        public List<Members> membersList{ get; set; }
        public List<SubContractor> scList { get; set; }
        public List<Roles> rolesList { get; set; }
        public List<Menu> menuList { get; set; }


    }
}