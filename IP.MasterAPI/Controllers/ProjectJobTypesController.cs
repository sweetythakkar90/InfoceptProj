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
   public  class ProjectJobTypesController : ApiController
    {
        private ProjectJobTypesService _ProjectJobTypesRepo = new ProjectJobTypesService();

        // GET: api/ProjectJobTypes
        [Route("api/ProjectJobTypes/Get/{projID}")]
        [HttpGet]
        public async Task<List<ProjectJobTypes>> Get(int projID)
        {
            List<ProjectJobTypes> lst = await Task.Run(() => _ProjectJobTypesRepo.GetProjectJobTypesDetailsAsync(0, projID));
            return lst;
           
        }
        // GET: api/ProjectJobTypes
        [Route("api/ProjectJobTypes/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] ProjectJobTypes projJobTypes)
        {
           
            _ProjectJobTypesRepo.InsertProjectJobTypesDetailsAsync(projJobTypes);

            var message = Request.CreateResponse(HttpStatusCode.Created, projJobTypes);
            message.Headers.Location = new Uri(Request.RequestUri + projJobTypes.Id.ToString());
            return message;

        }

        // GET: api/ProjectJobTypes
        [Route("api/ProjectJobTypes/Update")]
        [HttpPut]
        public async void Update([FromBody] ProjectJobTypes projJobTypes)
        {

            await Task.Run(() => _ProjectJobTypesRepo.UpdateProjectJobTypesDetailsAsync(projJobTypes));
            
        }

        // GET: api/ProjectJobTypes
        [Route("api/ProjectJobTypes/Delete/{Id}")]
        [HttpDelete]
        public async void Delete(int Id)
        {
            await Task.Run(() => _ProjectJobTypesRepo.DeleteProjectJobTypesDetailsAsync(Id));
            
        }
        
    }
}
