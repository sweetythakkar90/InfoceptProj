using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IP.JobsAPI.Services;
using IP.JobsAPI.Models;
using System.Threading.Tasks;


namespace IP.JobsAPI.Controllers
{
    public class JobStatusController : ApiController
    {
        private JobStatusService _JobStatusRepo = new JobStatusService();

        // GET: api/JobStatus
        [Route("api/JobStatus/Get")]
        [HttpGet]
        public async Task<List<JobStatus>> Get()
        {
            List<JobStatus> lst = await Task.Run(() => _JobStatusRepo.GetJobStatusDetailsAsync(0));
            return lst;
        }

        // GET: api/Company
        [Route("api/JobStatus/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] JobStatus jobStatus)
        {
            _JobStatusRepo.InsertJobStatusDetailsAsync(jobStatus);
            var message = Request.CreateResponse(HttpStatusCode.Created, jobStatus);
            message.Headers.Location = new Uri(Request.RequestUri + jobStatus.ID.ToString());
            return message;
        }

        // GET: api/Company
        [Route("api/JobStatus/Update")]
        [HttpPut]
        public async Task<List<JobStatus>> Update([FromBody] JobStatus jobStatus)
        {
            
            List<JobStatus> lst = await Task.Run(() => _JobStatusRepo.UpdateJobStatusDetailsAsync(jobStatus));
            return lst;
        }

        // GET: api/Company
        [Route("api/JobStatus/Delete/{ID}")]
        [HttpDelete]
        public async Task<List<JobStatus>> Delete(int ID)
        {
            List<JobStatus> lst = await Task.Run(() => _JobStatusRepo.DeleteJobStatusDetailsAsync(ID));
            return lst;
        }
    }
}
