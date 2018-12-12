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
   public  class ProjectSubContractorsController : ApiController
    {
        private ProjectSubContractorsService _ProjectSubContractorsRepo = new ProjectSubContractorsService();

        // GET: api/ProjectSubContractors
        [Route("api/ProjectSubContractors/Get/{projID}")]
        [HttpGet]
        public async Task<List<ProjectSubContractors>> Get(int projID)
        {
            List<ProjectSubContractors> lst = await Task.Run(() => _ProjectSubContractorsRepo.GetProjectSubContractorsDetailsAsync(0, projID));
            return lst;
           
        }
        // GET: api/ProjectSubContractors
        [Route("api/ProjectSubContractors/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] ProjectSubContractors projSubContractors)
        {
           
            _ProjectSubContractorsRepo.InsertProjectSubContractorsDetailsAsync(projSubContractors);

            var message = Request.CreateResponse(HttpStatusCode.Created, projSubContractors);
            message.Headers.Location = new Uri(Request.RequestUri + projSubContractors.Id.ToString());
            return message;

        }

        // GET: api/ProjectSubContractors
        [Route("api/ProjectSubContractors/Update")]
        [HttpPut]
        public async void Update([FromBody] ProjectSubContractors projSubContractors)
        {

            await Task.Run(() => _ProjectSubContractorsRepo.UpdateProjectSubContractorsDetailsAsync(projSubContractors));
            
        }

        // GET: api/ProjectSubContractors
        [Route("api/ProjectSubContractors/Delete/{Id}")]
        [HttpDelete]
        public async void Delete(int Id)
        {
            await Task.Run(() => _ProjectSubContractorsRepo.DeleteProjectSubContractorsDetailsAsync(Id));
            
        }
        
    }
}
