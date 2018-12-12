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
   public  class ProjectController : ApiController
    {
        private ProjectService _ProjectRepo = new ProjectService();

        // GET: api/Project
        [Route("api/Project/Get/{clientID}")]
        [HttpGet]
        public async Task<List<Project>> Get(int clientID)
        {
            List<Project> lst = await Task.Run(() => _ProjectRepo.GetProjectDetailsAsync(0, clientID));
            return lst;
           
        }
        // GET: api/Project
        [Route("api/Project/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] Project proj)
        {
           
            _ProjectRepo.InsertProjectDetailsAsync(proj);

            var message = Request.CreateResponse(HttpStatusCode.Created, proj);
            message.Headers.Location = new Uri(Request.RequestUri + proj.Id.ToString());
            return message;

        }

        // GET: api/Project
        [Route("api/Project/Update")]
        [HttpPut]
        public async Task<List<Project>> Update([FromBody] Project proj)
        {

            List<Project> lst = await Task.Run(() => _ProjectRepo.UpdateProjectDetailsAsync(proj));
            return lst;
        }

        // GET: api/Project
        [Route("api/Project/Delete/{Id}")]
        [HttpDelete]
        public async void Delete(int Id)
        {
            await Task.Run(() => _ProjectRepo.DeleteProjectDetailsAsync(Id));
        }
        
    }
}
