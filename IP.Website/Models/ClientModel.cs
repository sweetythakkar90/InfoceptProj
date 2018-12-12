using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IP.Website.Models
{
    public class ClientModel : GlobalModel
    {
        public int Id { get; set; }
        public int companyId{ get; set; }
        public string companyName{ get; set; }
        public string clientName  { get; set; }
        public string email { get; set; }

        public string  address { get; set; }

        //[DataType(DataType.PhoneNumber)]
        //[Display(Name = "Phone Number")]
        //[Required(ErrorMessage = "Phone Number Required!")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
        //           ErrorMessage = "Entered phone format is not valid.")]
        public string phoneNumber { get; set; }
     
        public int statusId { get; set; }

        public string statusName { get; set; }

        public List<StatusTypeModel> statusType { get; set; }

      
    }
}