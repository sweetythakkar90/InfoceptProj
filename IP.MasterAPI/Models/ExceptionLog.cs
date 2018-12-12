using System;
using System.ComponentModel.DataAnnotations;

namespace IP.MasterAPI.Models
{
    public class ExceptionLog : GlobalModel
    {
        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }
        public string ApplicationName { get; set; }
        public string MachineName { get; set; }
        public string ExceptionClassName { get; set; }
        public string ExceptionMethodName { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace{ get; set; }
        public string ServerName { get; set; }
        public string ExceptionType { get; set; }
        public string Url { get; set; }
        public Nullable<DateTime> ExceptionLoggingTime { get; set; }


    }
}