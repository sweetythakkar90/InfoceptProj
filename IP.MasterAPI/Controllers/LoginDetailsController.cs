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
   public  class LoginDetailsController : ApiController
    {
        private LoginDetailsService _LoginRepo = new LoginDetailsService();

        // GET: api/Login
        [Route("api/LoginDetails/Get")]
        [HttpGet]
        public async Task<List<LoginDetails>> Get()
        {
            List<LoginDetails> lst = await Task.Run(() => _LoginRepo.GetLoginDetailsAsync(0));
            return lst;
           
        }
       
        // GET: api/Login
        [Route("api/LoginDetails/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] LoginDetails ln)
        {
           
            _LoginRepo.InsertLoginDetailsAsync(ln);

            var message = Request.CreateResponse(HttpStatusCode.Created, ln);
            message.Headers.Location = new Uri(Request.RequestUri + ln.ID.ToString());
            return message;

        }

        // GET: api/Login
        [Route("api/LoginDetails/Update")]
        [HttpPut]
        public async Task<List<LoginDetails>> Update([FromBody] LoginDetails comp)
        {

            List<LoginDetails> lst = await Task.Run(() => _LoginRepo.UpdateLoginDetailsAsync(comp));
            return lst;
        }

        // GET: api/Login
        [Route("api/LoginDetails/Delete/{LoginID}")]
        [HttpDelete]
        public async Task<List<LoginDetails>> Delete(int LoginID)
        {
            List<LoginDetails> lst = await Task.Run(() => _LoginRepo.DeleteLoginDetailsAsync(LoginID));
            return lst;
        }
        
    }
}
