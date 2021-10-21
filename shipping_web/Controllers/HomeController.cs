using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shipping_Label_App.Data;
using Shipping_Label_App.Models;
using Shipping_Label_App.UtilityClasses;
using Shipping_Label_App.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Shipping_Label_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
       

        public HomeController(IWebHostEnvironment webHostEnvironment, ILogger<HomeController> logger)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult StaticWebsite()
        {
            return View();
        }
        private List<States> GetStates()
        {
            var rootpath = _webHostEnvironment.ContentRootPath;
            var jsonstring = System.IO.File.ReadAllText(rootpath + "/Data/StateList.json");
            var root = Newtonsoft.Json.JsonConvert.DeserializeObject<RootStateObject>(jsonstring);
            var statelist = root.States.ToList();
            return statelist;
        }
        public IActionResult Index()
        {
           SessionHelper.SetObjectAsJson(HttpContext.Session, "States", GetStates());
           return View();
        }
        private async Task<string> GetShippngRates(LabelVM model)
        {
            var itemlist = new List<items>();

            var item = new items
            {
                quantity = 1,
                category = "Mobile Phones",
                dimensions = new box
                {
                    length = model.Length,
                    width = model.Width,
                    height = model.Height
                },
                description = "Mobile Phones",
                actual_weight = 10,
                declared_currency = "USD",
                declared_customs_value = 500
            };

            itemlist.Add(item);

            var parcelslist = new List<parcels>();
            var parcels = new parcels
            {
                total_actual_weight = model.Weight,
                box = new box
                {
                    length = model.Length,
                    width = model.Width,
                    height = model.Height
                },
                items = itemlist
            };

            parcelslist.Add(parcels);
            var label = new LabelObject
            {
                origin_address = new origin_address
                {
                    line_1 = model.FromStreet,
                    line_2 = model.FromStreet2,
                    postal_code = model.FromZip,
                    state = model.FromState,
                    city = model.FromCity
                },
                destination_address = new destination_address
                {
                    line_1 = model.ToStreet,
                    line_2 = model.ToStreet2,
                    postal_code = model.ToZip,
                    state = model.ToState,
                    city = model.ToCity,
                    country_alpha2 = "US"
                },
                incoterms = "DDU",
                insurance = new insurance
                {
                    is_insured = false,
                    insured_amount = 10,
                    insured_currency = "USD"
                },
                courier_selection = new courier_selection
                {
                    apply_shipping_rules = true
                },
                shipping_settings = new shipping_settings
                {
                    units = new units
                    {
                        weight = "lb",
                        dimensions = "cm"
                    },
                    output_currency = "USD"
                },
                parcels = parcelslist
            };

            string json = JsonConvert.SerializeObject(label);
            string result = "";
            // In production code, don't destroy the HttpClient through using, but better reuse an existing instance
            // https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.easyship.com/v2/rates"))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "text/plain");
                    //request.Headers.TryAddWithoutValidation("Authorization", "Bearer sand_rwJZVpcl9/+D6FKRaXpzgnZcIf8ztG+Et6dRsD+0lYI=");
                    request.Headers.TryAddWithoutValidation("Authorization", "Bearer prod_LEpHYDF8oKqUGmNKEkiS7yURvdc+9j6VdJWGS7Shyok=");

                    request.Content = new StringContent(json);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    try
                    {
                        var response = await httpClient.SendAsync(request);
                        result = await response.Content.ReadAsStringAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                    }
                    finally
                    {
                        request.Dispose();
                    }
                }
            }
            return result;
        }
        [HttpPost]
        public async Task<IActionResult> RateOrder([FromBody] LabelVM labelvm)
        {
            var shippingrates = new ShipingRatesModel();
            try
            {
                string response = await GetShippngRates(labelvm);

                String remove = "\"remote_area_surcharges\":{},";

                response = response.Replace(remove, "");

                var jsonobj = JsonConvert.DeserializeObject<ShipingRatesModel>(response);

                shippingrates.rates = jsonobj.rates.OrderBy(s => s.total_charge).ToList();
                shippingrates.status = jsonobj.status;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            double rates = 0;

            try
            {
                foreach (var item in shippingrates.rates)
                {
                    rates = Convert.ToDouble(item.total_charge);
                    break;
                }
                var finalrate = (rates * 80) / 100;

                var ourrate = new Rate
                {
                    currency = "USD",
                    total_charge = finalrate,
                    courier_name = "STOPNSHIP Charges"
                };

                shippingrates.rates.Insert(0, ourrate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            var model = new LabelWithRates
            {
                labels = labelvm,
                ShipingRatesModel = shippingrates
            };

            ViewBag.RatesList = shippingrates;

            return Json(shippingrates);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
