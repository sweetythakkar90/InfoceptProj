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
    public class SORTypeController : ApiController
    {
        private SORTypeService _SORTypeRepo = new SORTypeService();

        // GET: api/SORType
        [Route("api/SORType/Get")]
        [HttpGet]
        public async Task<List<SORType>> Get()
        {
            List<SORType> lst = await Task.Run(() => _SORTypeRepo.GetSORTypeDetailsAsync(0));
            return lst;
        }

        // GET: api/Company
        [Route("api/SORType/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] SORType SORType)
        {
            _SORTypeRepo.InsertSORTypeDetailsAsync(SORType);
            var message = Request.CreateResponse(HttpStatusCode.Created, SORType);
            message.Headers.Location = new Uri(Request.RequestUri + SORType.ID.ToString());
            return message;
        }

        // GET: api/Company
        [Route("api/SORType/Update")]
        [HttpPut]
        public async Task<List<SORType>> Update([FromBody] SORType SORType)
        {
            
            List<SORType> lst = await Task.Run(() => _SORTypeRepo.UpdateSORTypeDetailsAsync(SORType));
            return lst;
        }

        // GET: api/Company
        [Route("api/SORType/Delete/{ID}")]
        [HttpDelete]
        public async Task<List<SORType>> Delete(int ID)
        {
            List<SORType> lst = await Task.Run(() => _SORTypeRepo.DeleteSORTypeDetailsAsync(ID));
            return lst;
        }

    }
}
