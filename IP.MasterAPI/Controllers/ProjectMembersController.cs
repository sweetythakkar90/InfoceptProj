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
   public  class ProjectMembersController : ApiController
    {
        private ProjectMembersService _ProjectMembersRepo = new ProjectMembersService();

        // GET: api/ProjectMembers
        [Route("api/ProjectMembers/Get/{projID}")]
        [HttpGet]
        public async Task<List<ProjectMembers>> Get(int projID)
        {
            List<ProjectMembers> lst = await Task.Run(() => _ProjectMembersRepo.GetProjectMembersDetailsAsync(0, projID));
            return lst;
           
        }
        // GET: api/ProjectMembers
        [Route("api/ProjectMembers/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] ProjectMembers projMembers)
        {
           
            _ProjectMembersRepo.InsertProjectMembersDetailsAsync(projMembers);

            var message = Request.CreateResponse(HttpStatusCode.Created, projMembers);
            message.Headers.Location = new Uri(Request.RequestUri + projMembers.Id.ToString());
            return message;

        }

        // GET: api/ProjectMembers
        [Route("api/ProjectMembers/Update")]
        [HttpPut]
        public async void Update([FromBody] ProjectMembers projMembers)
        {

            await Task.Run(() => _ProjectMembersRepo.UpdateProjectMembersDetailsAsync(projMembers));
            
        }

        // GET: api/ProjectMembers
        [Route("api/ProjectMembers/Delete/{Id}")]
        [HttpDelete]
        public async void Delete(int Id)
        {
            await Task.Run(() => _ProjectMembersRepo.DeleteProjectMembersDetailsAsync(Id));
            
        }
        
    }
}
