﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrainingManagementSystem.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound() => View();
        public ActionResult ServerError() => View();
    }
}