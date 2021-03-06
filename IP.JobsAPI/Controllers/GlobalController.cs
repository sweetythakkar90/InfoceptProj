﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IP.JobsAPI.Services;
using IP.JobsAPI.Models;
//using System.Web.Mvc;

namespace IP.JobsAPI.Controllers
{
   public  class GlobalController : ApiController
    {
        private GlobalService _GlobalRepo = new GlobalService();

        // GET: api/Global/CheckDataIntegrity
        [Route("api/Global/CheckDataIntegrity/{tableName}/{fieldName}/{fieldValue}/{ignoreTableList}")]
        [HttpGet]
        public int  CheckDataIntegrity(string tableName, string fieldName, int fieldValue,string ignoreTableList)
        {
            return _GlobalRepo.CheckDataIntegrity(tableName, fieldName, fieldValue, ignoreTableList);
        }
        // GET: api/Global/CheckDataIntegrity
        [Route("api/Global/CheckDataIntegrity/{tableName}/{fieldName}/{fieldValue}")]
        [HttpGet]
        public int CheckDataIntegrity(string tableName, string fieldName, int fieldValue)
        {
            return _GlobalRepo.CheckDataIntegrity(tableName, fieldName, fieldValue, string.Empty);
        }

    }
}
