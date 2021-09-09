using Microsoft.AspNetCore.Mvc;
using Shipping_Label_App.Data;
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
