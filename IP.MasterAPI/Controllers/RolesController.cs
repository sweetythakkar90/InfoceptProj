using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IP.MasterAPI.Services;
using IP.MasterAPI.Models;
using System.Threading.Tasks;


namespace IP.MasterAPI.Controllers
{
    public class RolesController : ApiController
    {
        private RolesService _RolesRepo = new RolesService();

        // GET: api/Roles
        [Route("api/Roles/Get")]
        [HttpGet]
        public async Task<List<Roles>> Get()
        {
            List<Roles> lst = await Task.Run(() => _RolesRepo.GetRolesDetailsAsync(0));
            return lst;
        }

        // GET: api/Roles
        [Route("api/Roles/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] Roles roles)
        {
            _RolesRepo.InsertRolesDetailsAsync(roles);
            var message = Request.CreateResponse(HttpStatusCode.Created, roles);
            message.Headers.Location = new Uri(Request.RequestUri + roles.Id.ToString());
            return message;
        }

        // GET: api/Roles
        [Route("api/Roles/Update")]
        [HttpPut]
        public async Task<List<Roles>> Update([FromBody] Roles roles)
        {

            List<Roles> lst = await Task.Run(() => _RolesRepo.UpdateRolesDetailsAsync(roles));
            return lst;
        }

        // GET: api/Roles
        [Route("api/Roles/Delete/{ID}")]
        [HttpDelete]
        public async Task<List<Roles>> Delete(int ID)
        {
            List<Roles> lst = await Task.Run(() => _RolesRepo.DeleteRolesDetailsAsync(ID));
            return lst;
        }
    }
}
