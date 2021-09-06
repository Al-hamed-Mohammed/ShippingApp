using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shipping_Label_App.Data;
using Shipping_Label_App.Models;
using Shipping_Label_App.UtilityClasses;

namespace Shipping_Label_App.Controllers
{
    public class MakeLabelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MakeLabelsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: MakeLabels
        public async Task<IActionResult> Index()
        {   
            return View(await _context.Labels.ToListAsync());
        }

        // GET: MakeLabels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labels = await _context.Labels
                .FirstOrDefaultAsync(m => m.LableID == id);
            if (labels == null)
            {
                return NotFound();
            }

            return View(labels);
        }

        // GET: MakeLabels/Create
        public IActionResult Create()
        {
            var model = new Labels();
            model.StatesList = GetStates();
            model.CountriesList = GetCountries();
            model.Providers = GetProviders();
            model.Classes = GetClasses();
            return View(model);
        }

        // POST: MakeLabels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("LableID,FromCountry,ToCountry,FromName,ToName,FromStreet,ToStreet,FromStreet2,ToStreet2,FromCity,ToCity,FromZip,ToZip,FromState,ToState,FromPhone,ToPhone,ProviderID,Weight,ClassId,SignatureRequired,Notes,SheduleEnable,SheduleDateTime")] Labels labels)
        public async Task<IActionResult> Create([Bind("LableID,FromCountry,ToCountry,FromName,ToName,FromStreet,ToStreet,FromStreet2,ToStreet2,FromCity,ToCity,FromZip,ToZip,FromState,ToState,FromPhone,ToPhone,ProviderID,Weight,ClassId,SignatureRequired,Notes,SheduleEnable,SheduleDateTime")] Labels labels)
        {
            if (ModelState.IsValid)
            {
                labels.Datecreated = DateTime.Now;
                labels.DateModified = DateTime.Now;
                labels.IsActive = false;
                labels.TrackingNo = null;

                _context.Add(labels);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            labels.StatesList = GetStates();
            labels.CountriesList = GetCountries();
            labels.Providers = GetProviders();
            labels.Classes = GetClasses();
            return View(labels);
        }

        // GET: MakeLabels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labels = await _context.Labels.FindAsync(id);
            if (labels == null)
            {
                return NotFound();
            }
            return View(labels);
        }

        // POST: MakeLabels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LableID,FromCountry,ToCountry,FromName,ToName,FromStreet,ToStreet,FromStreet2,ToStreet2,FromCity,ToCity,FromZip,ToZip,FromState,ToState,FromPhone,ToPhone,ProviderID,Weight,ClassId,SignatureRequired,Notes,SheduleEnable,SheduleDateTime,TrackingNo,Datecreated,DateModified,IsActive")] Labels labels)
        {
            if (id != labels.LableID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(labels);
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
                return RedirectToAction(nameof(Index));
            }
            return View(labels);
        }

        // GET: MakeLabels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labels = await _context.Labels
                .FirstOrDefaultAsync(m => m.LableID == id);
            if (labels == null)
            {
                return NotFound();
            }

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
    }
}
