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
   public  class CompanyController : ApiController
    {
        private CompanyService _companyRepo = new CompanyService();

        // GET: api/Company
        [Route("api/Company/Get")]
        [HttpGet]
        public async Task<List<Company>> Get()
        {
            List<Company> lst = await Task.Run(() => _companyRepo.GetCompanyDetailsAsync(0));
            return lst;
           
        }

        // GET: api/Company
        [Route("api/Company/Insert")]
        [HttpPost]
        public HttpResponseMessage Insert([FromBody] Company comp)
        {
           
            _companyRepo.InsertCompanyDetailsAsync(comp);

            var message = Request.CreateResponse(HttpStatusCode.Created, comp);
            message.Headers.Location = new Uri(Request.RequestUri + comp.companyID.ToString());
            return message;

        }

        // GET: api/Company
        [Route("api/Company/Update")]
        [HttpPut]
        public async Task<List<Company>> Update([FromBody] Company comp)
        {

            List<Company> lst = await Task.Run(() => _companyRepo.UpdateCompanyDetailsAsync(comp));
            return lst;
        }

        // GET: api/Company
        [Route("api/Company/Delete/{companyID}")]
        [HttpDelete]
        public async Task<List<Company>> Delete(int companyID)
        {
            List<Company> lst = await Task.Run(() => _companyRepo.DeleteCompanyDetailsAsync(companyID));
            return lst;
        }
        
    }
}
