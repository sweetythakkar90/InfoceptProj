﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IP.Website.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [Authorize]

        public ActionResult Index()
        {
            return View();
        }
    }
}