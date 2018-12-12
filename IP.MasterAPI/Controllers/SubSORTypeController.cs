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
    public class SubSORTypeController : ApiController
    {
        private SubSORTypeService _SubSORTypeRepo = new SubSORTypeService();

        // GET: api/SubSORType
        [Route("api/SubSORType/Get")]
        [HttpGet]
        public async Task<List<SubSORType>> Get()
        {
            List<SubSORType> lst = await Task.Run(() => _SubSORTypeRepo.GetSubSORTypeDetailsAsync(0));
            return lst;
        }
        [Route("api/SubSORType/GetSpecificSubSORType/{SORTypeId}")]
        [HttpGet]
        public async Task<List<SubSORType>> GetSpecificSubSORType(int SORTypeId)
        {
            List<SubSORType> lst = await Task.Run(() => _SubSORTypeRepo.GetSubSORTypeDetailsAsync(SORTypeId));
            return lst;
        }
        // GET: api/Company
        [Route("api/SubSORType/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] SubSORType SubSORType)
        {
            _SubSORTypeRepo.InsertSubSORTypeDetailsAsync(SubSORType);
            var message = Request.CreateResponse(HttpStatusCode.Created, SubSORType);
            message.Headers.Location = new Uri(Request.RequestUri + SubSORType.ID.ToString());
            return message;
        }

        // GET: api/Company
        [Route("api/SubSORType/Update")]
        [HttpPut]
        public async Task<List<SubSORType>> Update([FromBody] SubSORType SubSORType)
        {
            
            List<SubSORType> lst = await Task.Run(() => _SubSORTypeRepo.UpdateSubSORTypeDetailsAsync(SubSORType));
            return lst;
        }

        // GET: api/Company
        [Route("api/SubSORType/Delete/{ID}")]
        [HttpDelete]
        public async Task<List<SubSORType>> Delete(int ID)
        {
            List<SubSORType> lst = await Task.Run(() => _SubSORTypeRepo.DeleteSubSORTypeDetailsAsync(ID));
            return lst;
        }

    }
}
