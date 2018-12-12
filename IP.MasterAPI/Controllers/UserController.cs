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
   public  class UserController : ApiController
    {
        private AccountService _AccountRepo = new AccountService();

        // GET: api/User
        [Route("api/User/Get/{id}/{roleId}")]
        [HttpGet]
        public async Task<List<Account>> Get(int id,int roleId)
        {
            List<Account> lst = await Task.Run(() => _AccountRepo.GetUserDetailsAsync(id, roleId));
            return lst;

        }

        // GET: api/User
        [Route("api/User/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] Account user)
        {

            _AccountRepo.InsertUserDetailsAsync(user);

            var message = Request.CreateResponse(HttpStatusCode.Created, user);
            message.Headers.Location = new Uri(Request.RequestUri + user.Id.ToString());
            return message;

        }

        // GET: api/User
        [Route("api/User/Update")]
        [HttpPut]
        public void Update([FromBody] Account user)
        {

            Task.Run(() => _AccountRepo.UpdateUserDetailsAsync(user));
        }

        // GET: api/User
        [Route("api/User/Delete/{ID}")]
        [HttpDelete]
        public void Delete(int ID)
        {
            Task.Run(() => _AccountRepo.DeleteUserDetailsAsync(ID));
        }


    }
}
