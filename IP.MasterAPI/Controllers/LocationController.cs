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
   public  class LocationController : ApiController
    {
        private LocationService _LocationRepo = new LocationService();

        // GET: api/Location
        [Route("api/Location/Get")]
        [HttpGet]
        public async Task<List<Location>> Get()
        {
            List<Location> lst = await Task.Run(() => _LocationRepo.GetLocationDetailsAsync(0));
            return lst;
           
        }

        // GET: api/Location
        [Route("api/Location/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] Location comp)
        {
           
            _LocationRepo.InsertLocationDetailsAsync(comp);

            var message = Request.CreateResponse(HttpStatusCode.Created, comp);
            message.Headers.Location = new Uri(Request.RequestUri + comp.ID.ToString());
            return message;

        }

        // GET: api/Location
        [Route("api/Location/Update")]
        [HttpPut]
        public async Task<List<Location>> Update([FromBody] Location comp)
        {

            List<Location> lst = await Task.Run(() => _LocationRepo.UpdateLocationDetailsAsync(comp));
            return lst;
        }

        // GET: api/Location
        [Route("api/Location/Delete/{ID}")]
        [HttpDelete]
        public async Task<List<Location>> Delete(int ID)
        {
            List<Location> lst = await Task.Run(() => _LocationRepo.DeleteLocationDetailsAsync(ID));
            return lst;
        }
        
    }
}
