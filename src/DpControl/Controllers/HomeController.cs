﻿using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Controllers
{
    public class HomeController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}