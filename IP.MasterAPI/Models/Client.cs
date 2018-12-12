using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IP.MasterAPI.Models
{
    public class Client : GlobalModel
    {
        public int Id { get; set; }
        public int companyId{ get; set; }
        public string companyName { get; set; }
        public string clientName  { get; set; }
        public string email { get; set; }

        public string  address { get; set; }

        public string phoneNumber { get; set; }
     
        public int statusId { get; set; }
        public string statusName { get; set; }

        public List<StatusType> statusType { get; set; }
    }
}