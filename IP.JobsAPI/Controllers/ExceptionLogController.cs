using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IP.JobsAPI.Services;
using IP.JobsAPI.Models;
//using System.Web.Mvc;

namespace IP.JobsAPI.Controllers
{
   public  class ExceptionLogController : ApiController
    {
        private ExceptionLogService _ExceptionLogRepo = new ExceptionLogService();

        // GET: api/ExceptionLog
        [Route("api/ExceptionLog/Get")]
        [HttpGet]
        public async Task<List<ExceptionLog>> Get()
        {
            List<ExceptionLog> lst = await Task.Run(() => _ExceptionLogRepo.GetExceptionLogDetailsAsync(0));
            return lst;
           
        }

        // GET: api/ExceptionLog
        [Route("api/ExceptionLog/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] ExceptionLog eLog)
        {
           
            _ExceptionLogRepo.InsertExceptionLogDetailsAsync(eLog);

            var message = Request.CreateResponse(HttpStatusCode.Created, eLog);
            message.Headers.Location = new Uri(Request.RequestUri + eLog.ID.ToString());
            return message;

        }

        // GET: api/ExceptionLog
        [Route("api/ExceptionLog/Update")]
        [HttpPut]
        public async Task<List<ExceptionLog>> Update([FromBody] ExceptionLog eLog)
        {

            List<ExceptionLog> lst = await Task.Run(() => _ExceptionLogRepo.UpdateExceptionLogDetailsAsync(eLog));
            return lst;
        }

        // GET: api/ExceptionLog
        [Route("api/ExceptionLog/Delete/{ID}")]
        [HttpDelete]
        public async Task<List<ExceptionLog>> Delete(int ID)
        {
            List<ExceptionLog> lst = await Task.Run(() => _ExceptionLogRepo.DeleteExceptionLogDetailsAsync(ID));
            return lst;
        }
        
    }
}
