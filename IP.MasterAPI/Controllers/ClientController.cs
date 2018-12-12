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
   public  class ClientController : ApiController
    {
        private ClientService _ClientRepo = new ClientService();

        // GET: api/Client
        [Route("api/Client/Get")]
        [HttpGet]
        public async Task<List<Client>> Get()
        {
            List<Client> lst = await Task.Run(() => _ClientRepo.GetClientDetailsAsync(0));
            return lst;
           
        }
        // GET: api/Client
        [Route("api/Client/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] Client client)
        {
           
            _ClientRepo.InsertClientDetailsAsync(client);

            var message = Request.CreateResponse(HttpStatusCode.Created, client);
            message.Headers.Location = new Uri(Request.RequestUri + client.Id.ToString());
            return message;

        }

        // GET: api/Client
        [Route("api/Client/Update")]
        [HttpPut]
        public async Task<List<Client>> Update([FromBody] Client client)
        {

            List<Client> lst = await Task.Run(() => _ClientRepo.UpdateClientDetailsAsync(client));
            return lst;
        }

        // GET: api/Client
        [Route("api/Client/Delete/{Id}")]
        [HttpDelete]
        public async Task<List<Client>> Delete(int Id)
        {
            List<Client> lst = await Task.Run(() => _ClientRepo.DeleteClientDetailsAsync(Id));
            return lst;
        }
        
    }
}
