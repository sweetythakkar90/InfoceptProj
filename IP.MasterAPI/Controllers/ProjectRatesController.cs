using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IP.MasterAPI.Services;
using IP.MasterAPI.Models;
//using System.Web.Mvc;

namespace IP.MasterAPI.Controllers
{
   public  class ProjectRatesController : ApiController
    {
        private ProjectRatesService _ProjectRatesRepo = new ProjectRatesService();

        // GET: api/ProjectRates
        [Route("api/ProjectRates/Get/{projID}")]
        [HttpGet]
        public async Task<List<ProjectRates>> Get(int projID)
        {
            List<ProjectRates> lst = await Task.Run(() => _ProjectRatesRepo.GetProjectRatesDetailsAsync(0, projID));
            return lst;
           
        }
        // GET: api/ProjectRates
        [Route("api/ProjectRates/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] ProjectRates projRates)
        {
           
            _ProjectRatesRepo.InsertProjectRatesDetailsAsync(projRates);

            var message = Request.CreateResponse(HttpStatusCode.Created, projRates);
            message.Headers.Location = new Uri(Request.RequestUri + projRates.Id.ToString());
            return message;

        }

        // GET: api/ProjectRates
        [Route("api/ProjectRates/Update")]
        [HttpPut]
        public async void Update([FromBody] ProjectRates projRates)
        {

             await Task.Run(() => _ProjectRatesRepo.UpdateProjectRatesDetailsAsync(projRates));
        }

        // GET: api/ProjectRates
        [Route("api/ProjectRates/Delete/{ID}")]
        [HttpDelete]
        public async void Delete(int ID)
        {
            await Task.Run(() => _ProjectRatesRepo.DeleteProjectRatesDetailsAsync(ID));
            
        }
        
    }
}
