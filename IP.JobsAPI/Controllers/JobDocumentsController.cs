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
   public  class JobDocumentsController : ApiController
    {
        private JobDocumentsService _JobDocumentsRepo = new JobDocumentsService();

        // GET: api/JobDocuments
        [Route("api/JobDocuments/Get/{jobId}")]
        [HttpGet]
        public async Task<List<JobDocuments>> Get(int jobId)
        {
            List<JobDocuments> lst = await Task.Run(() => _JobDocumentsRepo.GetJobDocumentsDetailsAsync(0, jobId));
            return lst;
           
        }

        // GET: api/JobDocuments
        [Route("api/JobDocuments/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] JobDocuments JobDocuments)
        {
           
            _JobDocumentsRepo.InsertJobDocumentsDetailsAsync(JobDocuments);

            var message = Request.CreateResponse(HttpStatusCode.Created, JobDocuments);
            message.Headers.Location = new Uri(Request.RequestUri + JobDocuments.Id.ToString());
            return message;

        }

        // GET: api/JobDocuments
        [Route("api/JobDocuments/Update")]
        [HttpPut]
        public async void Update([FromBody] JobDocuments JobDocuments)
        {

            await Task.Run(() => _JobDocumentsRepo.UpdateJobDocumentsDetailsAsync(JobDocuments));
        }

        // GET: api/JobDocuments
        [Route("api/JobDocuments/Delete/{Id}")]
        [HttpDelete]
        public async void Delete(int Id)
        {
             await Task.Run(() => _JobDocumentsRepo.DeleteJobDocumentsDetailsAsync(Id));
        }
        
    }
}
