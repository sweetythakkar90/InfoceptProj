﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IP.Website.Controllers
{
    [Authorize]

    public class JobMenuController : Controller
    {
        // GET: MasterMenu
        public ActionResult Index()
        {
            return View();
        }
    }
}