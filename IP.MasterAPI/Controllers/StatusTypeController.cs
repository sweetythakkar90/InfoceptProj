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
    public class StatusTypeController : ApiController
    {
        private StatusTypeService _statusTypeRepo = new StatusTypeService();

        // GET: api/StatusType
        [Route("api/StatusType/Get")]
        [HttpGet]
        public async Task<List<StatusType>> Get()
        {
            List<StatusType> lst = await Task.Run(() => _statusTypeRepo.GetStatusTypeDetailsAsync(0));
            return lst;
        }

        // GET: api/Company
        [Route("api/StatusType/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] StatusType statusType)
        {
            _statusTypeRepo.InsertStatusTypeDetailsAsync(statusType);
            var message = Request.CreateResponse(HttpStatusCode.Created, statusType);
            message.Headers.Location = new Uri(Request.RequestUri + statusType.ID.ToString());
            return message;
        }

        // GET: api/Company
        [Route("api/StatusType/Update")]
        [HttpPut]
        public async Task<List<StatusType>> Update([FromBody] StatusType statusType)
        {
            
            List<StatusType> lst = await Task.Run(() => _statusTypeRepo.UpdateStatusTypeDetailsAsync(statusType));
            return lst;
        }

        // GET: api/Company
        [Route("api/StatusType/Delete/{ID}")]
        [HttpDelete]
        public async Task<List<StatusType>> Delete(int ID)
        {
            List<StatusType> lst = await Task.Run(() => _statusTypeRepo.DeleteStatusTypeDetailsAsync(ID));
            return lst;
        }
    }
}
