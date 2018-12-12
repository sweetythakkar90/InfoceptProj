using System;

namespace IP.MasterAPI.Models
{
    public class ProjectRates : GlobalModel
    {
        public int Id { get; set; }
        public int ProjId { get; set; }
        public int SORTypeId { get; set; }
        public string SORTypeName { get; set; }
        public int subSORTypeId { get; set; }
        public string subSORTypeName { get; set; }
        public string SORCode { get; set; }
        public string description { get; set; }
        public string unitOfMeasure { get; set; }
        public int unit { get; set; }
        public decimal unitPrice{ get; set; }
        public decimal cost{ get; set; }
        public DateTime expiryDate { get; set; }
        public int statusId { get; set; }
        public string statusName { get; set; }


    }
}