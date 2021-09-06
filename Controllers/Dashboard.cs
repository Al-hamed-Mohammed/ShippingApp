using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shipping_Label_App.Controllers
{
    public class Dashboard : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
