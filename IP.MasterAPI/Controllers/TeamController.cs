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
   public  class TeamController : ApiController
    {
        private TeamService _TeamRepo = new TeamService();

        // GET: api/Team
        [Route("api/Team/Get")]
        [HttpGet]
        public async Task<List<Team>> Get()
        {
            List<Team> lst = await Task.Run(() => _TeamRepo.GetTeamDetailsAsync(0,'I'));
            return lst;
           
        }
        // GET: api/Team
        [Route("api/Team/GetTeam/{TeamID}")]
        [HttpGet]
        public async Task<List<Team>> GetTeam(int TeamID)
        {
            List<Team> lst = await Task.Run(() => _TeamRepo.GetTeamDetailsAsync(TeamID, 'U'));
            return lst;

        }
        // GET: api/Team
        [Route("api/Team/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] Team tm)
        {

            _TeamRepo.InsertTeamDetailsAsync(tm, tm.memberMapping);

            var message = Request.CreateResponse(HttpStatusCode.Created, tm);
            message.Headers.Location = new Uri(Request.RequestUri + tm.Id.ToString());
            return message;

        }

        // GET: api/Team
        [Route("api/Team/Update")]
        [HttpPut]
        public async Task<List<Team>> Update([FromBody] Team tm)
        {

            List<Team> lst = await Task.Run(() => _TeamRepo.UpdateTeamDetailsAsync(tm, tm.memberMapping));
            return lst;
        }

        // GET: api/Team
        [Route("api/Team/Delete/{TeamID}")]
        [HttpDelete]
        public async Task<List<Team>> Delete(int TeamID)
        {
            List<Team> lst = await Task.Run(() => _TeamRepo.DeleteTeamDetailsAsync(TeamID));
            return lst;
        }
        [Route("api/TeamSkillSetMapping/Delete/{TeamID}")]
        [HttpDelete]
        public async Task<List<Team>> DeleteTeamSkillSetMapping(int TeamID)
        {
            List<Team> lst = await Task.Run(() => _TeamRepo.DeleteTeamDetailsAsync(TeamID));
            return lst;
        }
    }
}
