using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shipping_Label_App.Data;
using Shipping_Label_App.Models;
using Shipping_Label_App.UtilityClasses;
using Shipping_Label_App.ViewModel;

namespace Shipping_Label_App.Controllers
{
    [Authorize]
    public class MakeLabelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public MakeLabelsController(ApplicationDbContext context, 
            IWebHostEnvironment webHostEnvironment, 
            UserManager<ApplicationUser> userManager,
            ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllLabels()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            var stringrole = "";
            foreach (var role in roles)
            {
                stringrole = role;
            }
            var labellist = await _context.Labels.Select(s => new LabelVM
            {
                LableID = s.LableID,
                FromName = s.FromName,
                ToName = s.ToName,
                FromStreet = s.FromStreet,
                ToStreet = s.ToStreet,
                FromStreet2 = s.FromStreet2,
                ToStreet2 = s.ToStreet2,
                FromCity = s.FromCity,
                ToCity = s.ToCity,
                FromState = s.FromState,
                ToState = s.ToState,
                Provider = s.Provider,
                Class = s.Class,
                FromZip = s.FromZip,
                ToZip = s.ToZip,
                TrackingNo = s.TrackingNo,
                UserId = s.UserId,
                UserName = s.ApplicationUser == null ? "" : s.ApplicationUser.UserName,
                ProviderName = _context.providers.Where(x => x.Id == s.ProviderID).Select(z => z.ProviderName).FirstOrDefault(),
                ClassName = _context.classes.Where(x => x.Id == s.ClassId).Select(z => z.ClassName).FirstOrDefault(),
                RomeName = stringrole
            }).ToListAsync();
            return View(labellist);
        }

        [Authorize(Roles = "Admin ,User")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            var stringrole = "";
            foreach (var role in roles)
            {
                stringrole = role;
            }
            var list = await _context.Labels.Where(s => s.ApplicationUser == user).Select(s => new LabelVM
            {
                LableID = s.LableID,
                FromName = s.FromName,
                ToName = s.ToName,
                FromCity = s.FromCity,
                FromStreet = s.FromStreet,
                ToStreet = s.ToStreet,
                FromStreet2 = s.FromStreet2,
                ToStreet2 = s.ToStreet2,
                ToCity = s.ToCity,
                FromState = s.FromState,
                ToState = s.ToState,
                Provider = s.Provider,
                Class = s.Class,
                FromZip = s.FromZip,
                ToZip = s.ToZip,
                TrackingNo = s.TrackingNo,
                UserId = s.UserId,
                UserName = s.ApplicationUser == null ? "" : s.ApplicationUser.UserName,
                ProviderName = _context.providers.Where(x => x.Id == s.ProviderID).Select(z => z.ProviderName).FirstOrDefault(),
                ClassName = _context.classes.Where(x => x.Id == s.ClassId).Select(z => z.ClassName).FirstOrDefault(),
                RomeName = stringrole
            }).ToListAsync();

            //var user = await userManager.FindByIdAsync(userId);
            
            
            return View(list);
        }
        [Authorize(Roles = "Admin ,User")]
        // GET: MakeLabels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labels = await _context.Labels.FindAsync(id);
            labels.StatesList = GetStates();
            labels.CountriesList = GetCountries();
            labels.Providers = GetProviders();
            labels.Classes = GetClasses();
            if (labels == null)
            {
                return NotFound();
            }
            return View(labels);
        }
        [Authorize(Roles = "Admin ,User")]
        // GET: MakeLabels/Create
        public async Task<IActionResult> Create()
        {
            var model = new LabelVM();
            model.StatesList = GetStates();
            model.CountriesList = GetCountries();
            model.Providers = GetProviders();
            model.Classes = GetClasses();

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            var stringrole = "";
            foreach (var role in roles)
            {
                stringrole = role;
            }
            model.RomeName = stringrole;           

            return View(model);
        }

        // POST: MakeLabels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("LableID,FromCountry,ToCountry,FromName,ToName,FromStreet,ToStreet,FromStreet2,ToStreet2,FromCity,ToCity,FromZip,ToZip,FromState,ToState,FromPhone,ToPhone,ProviderID,Weight,ClassId,SignatureRequired,Notes,SheduleEnable,SheduleDateTime")] Labels labels)
        public async Task<IActionResult> Create([Bind("LableID,FromCountry,ToCountry,FromName,ToName,FromStreet,ToStreet,FromStreet2,ToStreet2,FromCity,ToCity,FromZip,ToZip,FromState,ToState,FromPhone,ToPhone,ProviderID,Weight,ClassId,SignatureRequired,Notes,SheduleEnable,SheduleDateTime,Length,Width,Height")] LabelVM labels)
        {
            if (ModelState.IsValid)
            {                                
                labels.Datecreated = DateTime.Now;
                labels.DateModified = DateTime.Now;
                labels.IsActive = false;
                labels.TrackingNo = null;

                return RedirectToAction("Order", labels);
            }

            labels.StatesList = GetStates();
            labels.CountriesList = GetCountries();
            labels.Providers = GetProviders();
            labels.Classes = GetClasses();
            return View(labels);
        }
        

        public async Task<IActionResult> Order(LabelVM labels)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            labels.ApplicationUser = user;
            var roles = await _userManager.GetRolesAsync(user);
            var stringrole = "";
            foreach (var role in roles)
            {
                stringrole = role;
            }

            var shippingrates = new ShipingRatesModel();
            try
            {
                string response = await GetShippngRates(labels);

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
                labels = labels,
                ShipingRatesModel = shippingrates
            };
            
            ViewBag.RatesList = shippingrates;

            return View(model);
        }

        [HttpPost, ActionName("Order")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderPost([Bind("LableID,FromCountry,ToCountry,FromName,ToName,FromStreet,ToStreet,FromStreet2,ToStreet2,FromCity,ToCity,FromZip,ToZip,FromState,ToState,FromPhone,ToPhone,ProviderID,Weight,ClassId,SignatureRequired,Notes,SheduleEnable,SheduleDateTime,Length,Width,Height,CarrierName,ShipingRate")] LabelVM labels, string couriername, string totalcharges)
        {
            if (ModelState.IsValid)
            {
                labels.Datecreated = DateTime.Now;
                labels.DateModified = DateTime.Now;
                labels.IsActive = false;
                labels.TrackingNo = null;
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var label = new Labels
                {
                    LableID = labels.LableID,
                    FromCountry = labels.FromCountry,
                    ToCountry = labels.ToCountry,
                    FromName = labels.FromName,
                    ToName = labels.ToName,
                    FromCity = labels.FromCity,
                    FromStreet = labels.FromStreet,
                    ToStreet = labels.ToStreet,
                    FromStreet2 = labels.FromStreet2,
                    ToStreet2 = labels.ToStreet2,
                    ToCity = labels.ToCity,
                    FromState = labels.FromState,
                    ToState = labels.ToState,
                    Provider = labels.Provider,
                    Class = labels.Class,
                    FromZip = labels.FromZip,
                    ToZip = labels.ToZip,
                    TrackingNo = labels.TrackingNo,
                    UserId = user.Id,
                    FromPhone = labels.FromPhone,
                    ToPhone = labels.ToPhone,
                    ProviderID = labels.ProviderID,
                    ClassId = labels.ClassId,
                    Weight = labels.Weight,
                    Length = labels.Length,
                    Width = labels.Width,
                    Height = labels.Height,
                    SheduleEnable = labels.SheduleEnable,
                    SheduleDateTime = labels.SheduleDateTime,
                    CarrierName = couriername,
                    ShipingRate = Convert.ToDouble(totalcharges)
                };

                _context.Labels.Add(label);
                await _context.SaveChangesAsync();

                
                var roles = await _userManager.GetRolesAsync(user);
                var stringrole = "";
                foreach (var role in roles)
                {
                    stringrole = role;
                }

                if (stringrole == "Admin")
                    return RedirectToAction(nameof(AllLabels));
                else
                    return RedirectToAction(nameof(Index));
            }         
            return View(labels);
        }

        [Authorize(Roles = "Admin")]
        // GET: MakeLabels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labels = await _context.Labels.FindAsync(id);
            labels.StatesList = GetStates();
            labels.CountriesList = GetCountries();
            labels.Providers = GetProviders();
            labels.Classes = GetClasses();
            if (labels == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            var stringrole = "";
            foreach (var role in roles)
            {
                stringrole = role;
            }
            labels.RomeName = stringrole;
            return View(labels);
        }

        // POST: MakeLabels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.0
        //LableID,FromCountry,ToCountry,FromName,ToName,FromStreet,ToStreet,FromStreet2,ToStreet2,FromCity,ToCity,FromZip,ToZip,FromState,ToState,FromPhone,ToPhone,ProviderID,Weight,ClassId,SignatureRequired,Notes,SheduleEnable,SheduleDateTime,TrackingNo,Datecreated,DateModified,IsActive
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LableID,FromCountry,ToCountry,FromName,ToName,FromStreet,ToStreet,FromStreet2,ToStreet2,FromCity,ToCity,FromZip,ToZip,FromState,ToState,FromPhone,ToPhone,ProviderID,Weight,ClassId,SignatureRequired,Notes,SheduleEnable,SheduleDateTime,TrackingNo,Datecreated,DateModified,IsActive,Length,Width,Height,CarrierName,ShipingRate")] Labels labels)
        {
            if (id != labels.LableID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var labelitem = await _context.Labels.FirstOrDefaultAsync(l => l.LableID == id);
                    if (labelitem == null)
                        return NotFound();

                    labelitem.TrackingNo = labels.TrackingNo;
                    labelitem.DateModified = DateTime.Now;
                    _context.Update(labelitem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabelsExists(labels.LableID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var roles = await _userManager.GetRolesAsync(user);
                var stringrole = "";
                foreach (var role in roles)
                {
                    stringrole = role;
                }

                if (stringrole == "Admin")
                    return RedirectToAction(nameof(AllLabels));
                else
                    return RedirectToAction(nameof(Index));
            }
            return View(labels);
        }

        // GET: MakeLabels/Delete/5
        [Authorize(Roles = "Admin ,User")]
        public async Task<IActionResult> Delete(int? Id)
        {          

            if (Id == null)
            {
                return NotFound();
            }

            var labels = await _context.Labels.FindAsync(Id);
            labels.StatesList = GetStates();
            labels.CountriesList = GetCountries();
            labels.Providers = GetProviders();
            labels.Classes = GetClasses();
            if (labels == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            var stringrole = "";
            foreach (var role in roles)
            {
                stringrole = role;
            }
            labels.RomeName = stringrole;
            return View(labels);
        }

        // POST: MakeLabels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var labels = await _context.Labels.FindAsync(id);
            _context.Labels.Remove(labels);
            await _context.SaveChangesAsync();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            var stringrole = "";
            foreach (var role in roles)
            {
                stringrole = role;
            }

            if (stringrole == "Admin")
                return RedirectToAction(nameof(AllLabels));
            else
                return RedirectToAction(nameof(Index));
        }

        private bool LabelsExists(int id)
        {
            return _context.Labels.Any(e => e.LableID == id);
        }

        #region All Helper Functions are here

        private List<States> GetStates()
        {
            var rootpath = _webHostEnvironment.ContentRootPath;
            var jsonstring = System.IO.File.ReadAllText(rootpath + "/Data/StateList.json");
            var root = Newtonsoft.Json.JsonConvert.DeserializeObject<RootStateObject>(jsonstring);
            var statelist = root.States.ToList();

            return statelist;
        }

        private List<Country> GetCountries()
        {
            var rootpath = _webHostEnvironment.ContentRootPath;
            var jsonstring = System.IO.File.ReadAllText(rootpath + "/Data/CountryList.json");
            var root = Newtonsoft.Json.JsonConvert.DeserializeObject<RootCountryObject>(jsonstring);
            var countrylist = root.Countries.ToList();

            return countrylist;
        }

        private List<Providers> GetProviders()
        {
            return _context.providers.ToList();
        }

        private List<Classes> GetClasses()
        {
            return _context.classes.ToList();
        }

        #endregion

        private async Task<string> GetShippngRates(LabelVM model)
        {
            var itemlist = new List<items>();

            var item = new items
            {
                quantity = 1,
                category = "Mobile Phones",
                dimensions = new box
                {
                    length =model.Length,
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
    }
}
