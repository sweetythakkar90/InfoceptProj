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
   public  class MenuRolesRelationController : ApiController
    {
        private MenuRolesRelationService _MenuRolesRelationRepo = new MenuRolesRelationService();

        // GET: api/MenuRolesRelation
        [Route("api/MenuRolesRelation/Get/{roleId}")]
        [HttpGet]
        public async Task<List<MenuRolesRelation>> Get(int roleId)
        {
            List<MenuRolesRelation> lst = await Task.Run(() => _MenuRolesRelationRepo.GetMenuRolesRelationDetailsAsync(0, roleId));
            return lst;
           
        }
        // GET: api/MenuRolesRelation
        [Route("api/MenuRolesRelation/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] MenuRolesRelation projRates)
        {
           
            _MenuRolesRelationRepo.InsertMenuRolesRelationDetailsAsync(projRates);

            var message = Request.CreateResponse(HttpStatusCode.Created, projRates);
            message.Headers.Location = new Uri(Request.RequestUri + projRates.Id.ToString());
            return message;

        }

        // GET: api/MenuRolesRelation
        [Route("api/MenuRolesRelation/Update")]
        [HttpPut]
        public async void Update([FromBody] MenuRolesRelation projRates)
        {

             await Task.Run(() => _MenuRolesRelationRepo.UpdateMenuRolesRelationDetailsAsync(projRates));
        }

        // GET: api/MenuRolesRelation
        [Route("api/MenuRolesRelation/Delete/{ID}")]
        [HttpDelete]
        public async void Delete(int ID)
        {
            await Task.Run(() => _MenuRolesRelationRepo.DeleteMenuRolesRelationDetailsAsync(ID));
            
        }
        
    }
}
