using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IP.Website.Models
{
    public class AccountModel : GlobalModel
    {
        public int Id { get; set; }
        [Required]
        public int uId { get; set; }
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }

        public int memberId { get; set; }
        public string memberName{ get; set; }
        public int subcontractorId { get; set; }
        public string subcontractorName { get; set; }

        public int roleId { get; set; }

        public string roles { get; set; }

        public string userType { get; set; }

        public int statusId { get; set; }
        public string statusName { get; set; }

        public List<StatusTypeModel> statusTypeList { get; set; }
        public List<MembersModel> membersList { get; set; }
        public List<SubContractorModel> scList { get; set; }
        public List<RolesModel> rolesList { get; set; }
        public List<MenuModel> menuList { get; set; }
    }
}