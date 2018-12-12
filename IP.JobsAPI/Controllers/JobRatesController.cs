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
   public  class JobRatesController : ApiController
    {
        private JobRatesService _JobRatesRepo = new JobRatesService();

        // GET: api/JobRates
        [Route("api/JobRates/Get/{projId}")]
        [HttpGet]
        public async Task<List<JobRates>> Get(int jobId)
        {
            List<JobRates> lst = await Task.Run(() => _JobRatesRepo.GetJobRatesDetailsAsync(0, jobId));
            return lst;
           
        }

        // GET: api/JobRates
        [Route("api/JobRates/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] JobRates JobRates)
        {
           
            _JobRatesRepo.InsertJobRatesDetailsAsync(JobRates);

            var message = Request.CreateResponse(HttpStatusCode.Created, JobRates);
            message.Headers.Location = new Uri(Request.RequestUri + JobRates.Id.ToString());
            return message;

        }

        // GET: api/JobRates
        [Route("api/JobRates/Update")]
        [HttpPut]
        public async void Update([FromBody] JobRates JobRates)
        {

            await Task.Run(() => _JobRatesRepo.UpdateJobRatesDetailsAsync(JobRates));
        }

        // GET: api/JobRates
        [Route("api/JobRates/Delete/{Id}")]
        [HttpDelete]
        public async void Delete(int Id)
        {
             await Task.Run(() => _JobRatesRepo.DeleteJobRatesDetailsAsync(Id));
        }
        
    }
}
