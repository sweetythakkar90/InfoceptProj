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
   public  class JobsController : ApiController
    {
        private JobsService _JobsRepo = new JobsService();

        // GET: api/Jobs
        [Route("api/Jobs/Get/{Id}")]
        [HttpGet]
        public async Task<List<Jobs>> Get(int Id)
        {
            List<Jobs> lst = await Task.Run(() => _JobsRepo.GetJobsDetailsAsync(0, Id));
            return lst;
           
        }

        // GET: api/Jobs
        [Route("api/Jobs/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] Jobs jobs)
        {
           
            _JobsRepo.InsertJobsDetailsAsync(jobs);

            var message = Request.CreateResponse(HttpStatusCode.Created, jobs);
            message.Headers.Location = new Uri(Request.RequestUri + jobs.Id.ToString());
            return message;

        }

        // GET: api/Jobs
        [Route("api/Jobs/Update")]
        [HttpPut]
        public async Task<List<Jobs>> Update([FromBody] Jobs jobs)
        {

            List<Jobs> lst = await Task.Run(() => _JobsRepo.UpdateJobsDetailsAsync(jobs));
            return lst;
        }

        // GET: api/Jobs
        [Route("api/Jobs/Delete/{Id}")]
        [HttpDelete]
        public async void Delete(int Id)
        {
             await Task.Run(() => _JobsRepo.DeleteJobsDetailsAsync(Id));
        }
        
    }
}
