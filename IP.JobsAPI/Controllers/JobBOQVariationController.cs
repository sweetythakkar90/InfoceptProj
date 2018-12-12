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
   public  class JobBOQVariationController : ApiController
    {
        private JobBOQVariationService _JobBOQVariationRepo = new JobBOQVariationService();

        // GET: api/JobBOQVariation
        [Route("api/JobBOQVariation/Get/{jobId}")]
        [HttpGet]
        public async Task<List<JobBOQVariation>> Get(int jobId)
        {
            List<JobBOQVariation> lst = await Task.Run(() => _JobBOQVariationRepo.GetJobBOQVariationDetailsAsync(0, jobId));
            return lst;
           
        }

        // GET: api/JobBOQVariation
        [Route("api/JobBOQVariation/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] JobBOQVariation JobBOQVariation)
        {
           
            _JobBOQVariationRepo.InsertJobBOQVariationDetailsAsync(JobBOQVariation);

            var message = Request.CreateResponse(HttpStatusCode.Created, JobBOQVariation);
            message.Headers.Location = new Uri(Request.RequestUri + JobBOQVariation.Id.ToString());
            return message;

        }

        // GET: api/JobBOQVariation
        [Route("api/JobBOQVariation/Update")]
        [HttpPut]
        public async void Update([FromBody] JobBOQVariation JobBOQVariation)
        {

            await Task.Run(() => _JobBOQVariationRepo.UpdateJobBOQVariationDetailsAsync(JobBOQVariation));
        }

        // GET: api/JobBOQVariation
        [Route("api/JobBOQVariation/Delete/{Id}")]
        [HttpDelete]
        public async void Delete(int Id)
        {
             await Task.Run(() => _JobBOQVariationRepo.DeleteJobBOQVariationDetailsAsync(Id));
        }
        
    }
}
