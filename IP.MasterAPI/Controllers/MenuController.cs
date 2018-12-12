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
   public  class MenuController : ApiController
    {
        private MenuService _MenuRepo = new MenuService();

        // GET: api/Menu
        [Route("api/Menu/Get/{roleId}/{mode}")]
        [HttpGet]
        public List<Menu> Get(int roleId, char mode)
        {
            List<Menu> lst = _MenuRepo.GetMenuDetails(roleId, mode);
            return lst;
           
        }
    }
}
