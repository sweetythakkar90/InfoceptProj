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
   public  class SubContractorController : ApiController
    {
        private SubContractorService _SubContractorRepo = new SubContractorService();

        // GET: api/SubContractor
        [Route("api/SubContractor/Get")]
        [HttpGet]
        public async Task<List<SubContractor>> Get()
        {
            List<SubContractor> lst = await Task.Run(() => _SubContractorRepo.GetSubContractorDetailsAsync(0,'I'));
            return lst;

        }  
        // GET: api/Members
        [Route("api/SubContractor/GetSubContractor/{subContractorID}")]
        [HttpGet]
        public async Task<List<SubContractor>> GetSubContractor(int subContractorID)
        {
            List<SubContractor> lst = await Task.Run(() => _SubContractorRepo.GetSubContractorDetailsAsync(subContractorID, 'U'));
            return lst;

        }

        // GET: api/SubContractor
        [Route("api/SubContractor/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] SubContractor sc)
        {

            _SubContractorRepo.InsertSubContractorDetailsAsync(sc, sc.skillSelect);

            var message = Request.CreateResponse(HttpStatusCode.Created, sc);
            message.Headers.Location = new Uri(Request.RequestUri + sc.Id.ToString());
            return message;

        }

        // GET: api/SubContractor
        [Route("api/SubContractor/Update")]
        [HttpPut]
        public async Task<List<SubContractor>> Update([FromBody] SubContractor sc)
        {

            List<SubContractor> lst = await Task.Run(() => _SubContractorRepo.UpdateSubContractorDetailsAsync(sc,sc.skillSelect));
            return lst;
        }

        // GET: api/SubContractor
        [Route("api/SubContractor/Delete/{SubContractorID}")]
        [HttpDelete]
        public async Task<List<SubContractor>> Delete(int SubContractorID)
        {
            List<SubContractor> lst = await Task.Run(() => _SubContractorRepo.DeleteSubContractorDetailsAsync(SubContractorID));
            return lst;
        }

        [Route("api/SubContractorRates/Delete/{subConRateID}")]
        [HttpDelete]
        public async Task<List<SubContractor>> DeleteSubContractorRatesAsync(int subConRateID)
        {
            List<SubContractor> lst = await Task.Run(() => _SubContractorRepo.DeleteSubContractorRatesAsync(subConRateID));
            return lst;
        }

        [Route("api/SubContractorSkillSetMapping/Delete/{SubContractorID}")]
        [HttpDelete]
        public async Task<List<SubContractor>> DeleteSubContractorSkillSetMapping(int SubContractorID)
        {
            List<SubContractor> lst = await Task.Run(() => _SubContractorRepo.DeleteSubContractorDetailsAsync(SubContractorID));
            return lst;
        }
    }
}
