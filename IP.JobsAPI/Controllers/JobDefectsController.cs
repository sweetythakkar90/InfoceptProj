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
   public  class JobDefectsController : ApiController
    {
        private JobDefectsService _JobDefectsRepo = new JobDefectsService();

        // GET: api/JobDefects
        [Route("api/JobDefects/Get/{jobId}")]
        [HttpGet]
        public async Task<List<JobDefects>> Get(int jobId)
        {
            List<JobDefects> lst = await Task.Run(() => _JobDefectsRepo.GetJobDefectsDetailsAsync(0, jobId));
            return lst;
           
        }

        // GET: api/JobDefects
        [Route("api/JobDefects/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] JobDefects JobDefects)
        {
           
            _JobDefectsRepo.InsertJobDefectsDetailsAsync(JobDefects);

            var message = Request.CreateResponse(HttpStatusCode.Created, JobDefects);
            message.Headers.Location = new Uri(Request.RequestUri + JobDefects.Id.ToString());
            return message;

        }

        // GET: api/JobDefects
        [Route("api/JobDefects/Update")]
        [HttpPut]
        public async void Update([FromBody] JobDefects JobDefects)
        {

            await Task.Run(() => _JobDefectsRepo.UpdateJobDefectsDetailsAsync(JobDefects));
        }

        // GET: api/JobDefects
        [Route("api/JobDefects/Delete/{Id}")]
        [HttpDelete]
        public async void Delete(int Id)
        {
             await Task.Run(() => _JobDefectsRepo.DeleteJobDefectsDetailsAsync(Id));
        }
        
    }
}
