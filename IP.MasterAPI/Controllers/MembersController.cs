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
   public  class MembersController : ApiController
    {
        private MembersService _MembersRepo = new MembersService();

        // GET: api/Members
        [Route("api/Members/Get")]
        [HttpGet]
        public async Task<List<Members>> Get()
        {
            List<Members> lst = await Task.Run(() => _MembersRepo.GetMembersDetailsAsync(0,'I'));
            return lst;
           
        }
        // GET: api/Members
        [Route("api/Members/GetMember/{memberID}")]
        [HttpGet]
        public async Task<List<Members>> GetMember(int memberID)
        {
            List<Members> lst = await Task.Run(() => _MembersRepo.GetMembersDetailsAsync(memberID, 'U'));
            return lst;

        }
        // GET: api/Members
        [Route("api/Members/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] Members mem)
        {

            _MembersRepo.InsertMembersDetailsAsync(mem, mem.skillSelect);

            var message = Request.CreateResponse(HttpStatusCode.Created, mem);
            message.Headers.Location = new Uri(Request.RequestUri + mem.Id.ToString());
            return message;

        }

        // GET: api/Members
        [Route("api/Members/Update")]
        [HttpPut]
        public async Task<List<Members>> Update([FromBody] Members mem)
        {

            List<Members> lst = await Task.Run(() => _MembersRepo.UpdateMembersDetailsAsync(mem, mem.skillSelect));
            return lst;
        }

        // GET: api/Members
        [Route("api/Members/Delete/{MembersID}")]
        [HttpDelete]
        public async Task<List<Members>> Delete(int MembersID)
        {
            List<Members> lst = await Task.Run(() => _MembersRepo.DeleteMembersDetailsAsync(MembersID));
            return lst;
        }
        [Route("api/MembersSkillSetMapping/Delete/{MembersID}")]
        [HttpDelete]
        public async Task<List<Members>> DeleteMembersSkillSetMapping(int MembersID)
        {
            List<Members> lst = await Task.Run(() => _MembersRepo.DeleteMembersDetailsAsync(MembersID));
            return lst;
        }
    }
}
