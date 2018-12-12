using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IP.Website.Models;
using System.Diagnostics;

namespace IP.Website.Exceptions
{
    public static class ExceptionLogHandler
    {
        // GET: ExceptionLogType
        static  string Baseurl = "http://ipmasterapi-dev.ap-southeast-2.elasticbeanstalk.com/";
        //static string Baseurl = "http://localhost:57225/";
        public static  ExceptionLogModel CreateLogData(Exception ex)
        {
            ExceptionLogModel log = new ExceptionLogModel();
            log.UserID = 1;
            log.ApplicationName = "PMS";
            log.MachineName = HttpContext.Current.Server.MachineName;
            log.ExceptionClassName = new StackTrace(ex).GetFrame(0).GetMethod().DeclaringType.Name.ToString();
            log.ExceptionMethodName = new StackTrace(ex).GetFrame(0).GetMethod().Name;

            log.ExceptionMessage = ex.Message;
            log.ExceptionStackTrace = ex.StackTrace;
            log.ServerName = HttpContext.Current.Server.MachineName;
            log.ExceptionType = "E";
            log.Url = HttpContext.Current.Request.Url.AbsoluteUri;
            log.ExceptionLoggingTime = DateTime.Now;

            return log;
        }
        public static void LogData( Exception ex)
        {
            ExceptionLogModel ExceptionLog = CreateLogData(ex);
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();

                var stype =  JsonConvert.SerializeObject(ExceptionLog);

                // Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllComapnies using HttpClient 
                //var Res = client.PostAsync("api/ExceptionLog/insert", new StringContent(stype,Encoding.UTF8,"application/json"));
                var resp = client.PostAsJsonAsync<ExceptionLogModel>("api/ExceptionLog/insert", ExceptionLog).Result;


            }
        }

       
    }
}