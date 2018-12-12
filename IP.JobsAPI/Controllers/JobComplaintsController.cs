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
   public  class JobComplaintsController : ApiController
    {
        private JobComplaintsService _JobComplaintsRepo = new JobComplaintsService();

        // GET: api/JobComplaints
        [Route("api/JobComplaints/Get/{jobId}")]
        [HttpGet]
        public async Task<List<JobComplaints>> Get(int jobId)
        {
            List<JobComplaints> lst = await Task.Run(() => _JobComplaintsRepo.GetJobComplaintsDetailsAsync(0, jobId));
            return lst;
           
        }

        // GET: api/JobComplaints
        [Route("api/JobComplaints/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] JobComplaints JobComplaints)
        {
           
            _JobComplaintsRepo.InsertJobComplaintsDetailsAsync(JobComplaints);

            var message = Request.CreateResponse(HttpStatusCode.Created, JobComplaints);
            message.Headers.Location = new Uri(Request.RequestUri + JobComplaints.Id.ToString());
            return message;

        }

        // GET: api/JobComplaints
        [Route("api/JobComplaints/Update")]
        [HttpPut]
        public async void Update([FromBody] JobComplaints JobComplaints)
        {

            await Task.Run(() => _JobComplaintsRepo.UpdateJobComplaintsDetailsAsync(JobComplaints));
        }

        // GET: api/JobComplaints
        [Route("api/JobComplaints/Delete/{Id}")]
        [HttpDelete]
        public async void Delete(int Id)
        {
             await Task.Run(() => _JobComplaintsRepo.DeleteJobComplaintsDetailsAsync(Id));
        }
        
    }
}
