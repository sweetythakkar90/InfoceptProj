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
   public  class JobAssignmentController : ApiController
    {
        private JobAssignmentService _JobAssignmentRepo = new JobAssignmentService();

        // GET: api/JobAssignment
        [Route("api/JobAssignment/Get/{jobId}")]
        [HttpGet]
        public async Task<List<JobAssignment>> Get(int jobId)
        {
            List<JobAssignment> lst = await Task.Run(() => _JobAssignmentRepo.GetJobAssignmentDetailsAsync(0, jobId));
            return lst;
           
        }

        // GET: api/JobAssignment
        [Route("api/JobAssignment/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] JobAssignment JobAssignment)
        {
           
            _JobAssignmentRepo.InsertJobAssignmentDetailsAsync(JobAssignment);

            var message = Request.CreateResponse(HttpStatusCode.Created, JobAssignment);
            message.Headers.Location = new Uri(Request.RequestUri + JobAssignment.Id.ToString());
            return message;

        }

        // GET: api/JobAssignment
        [Route("api/JobAssignment/Update")]
        [HttpPut]
        public async void Update([FromBody] JobAssignment jobAssignment)
        {

            await Task.Run(() => _JobAssignmentRepo.UpdateJobAssignmentDetailsAsync(jobAssignment));
        }

        // GET: api/JobAssignment
        [Route("api/JobAssignment/Delete/{Id}")]
        [HttpDelete]
        public async void Delete(int Id)
        {
             await Task.Run(() => _JobAssignmentRepo.DeleteJobAssignmentDetailsAsync(Id));
        }
        
    }
}
