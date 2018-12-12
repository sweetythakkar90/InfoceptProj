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
   public  class AccountController : ApiController
    {
        private AccountService _AccountRepo = new AccountService();

        // GET: api/Account
        [Route("api/Account/Get/{userName}/{password}")]
        [HttpGet]
        public Account Get(string userName, string password)
        {
            Account lst = _AccountRepo.GetAuthenticateUser(userName, password);
            return lst;
           
        }

        
    }
}
