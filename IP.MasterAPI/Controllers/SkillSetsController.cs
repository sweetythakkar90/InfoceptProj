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
    public class SkillSetsController : ApiController
    {
        private SkillSetsService _SkillSetsRepo = new SkillSetsService();

        // GET: api/SkillSets
        [Route("api/SkillSets/Get")]
        [HttpGet]
        public async Task<List<SkillSets>> Get()
        {
            List<SkillSets> lst = await Task.Run(() => _SkillSetsRepo.GetSkillSetsDetailsAsync(0));
            return lst;
        }

        // GET: api/Company
        [Route("api/SkillSets/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] SkillSets SkillSets)
        {
            _SkillSetsRepo.InsertSkillSetsDetailsAsync(SkillSets);
            var message = Request.CreateResponse(HttpStatusCode.Created, SkillSets);
            message.Headers.Location = new Uri(Request.RequestUri + SkillSets.ID.ToString());
            return message;
        }

        // GET: api/Company
        [Route("api/SkillSets/Update")]
        [HttpPut]
        public async Task<List<SkillSets>> Update([FromBody] SkillSets SkillSets)
        {
            
            List<SkillSets> lst = await Task.Run(() => _SkillSetsRepo.UpdateSkillSetsDetailsAsync(SkillSets));
            return lst;
        }

        // GET: api/Company
        [Route("api/SkillSets/Delete/{ID}")]
        [HttpDelete]
        public async Task<List<SkillSets>> Delete(int ID)
        {
            List<SkillSets> lst = await Task.Run(() => _SkillSetsRepo.DeleteSkillSetsDetailsAsync(ID));
            return lst;
        }

    }
}
