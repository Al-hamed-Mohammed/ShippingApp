﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        public MakeLabelsController(ApplicationDbContext context, 
            IWebHostEnvironment webHostEnvironment, 
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllLabels()
        {

            var labellist = await _context.Labels.Select(s => new LabelVM
            {
                LableID = s.LableID,
                FromName = s.FromName,
                ToName = s.ToName,
                FromCity = s.FromCity,
                ToCity = s.ToCity,
                FromState = s.FromState,
                ToState = s.ToState,
                Provider = s.Provider,
                Class = s.Class,
                TrackingNo = s.TrackingNo,
                UserId = s.UserId,
                UserName = s.ApplicationUser == null ? "" : s.ApplicationUser.UserName,
                ProviderName = _context.providers.Where(x => x.Id == s.ProviderID).Select(z => z.ProviderName).FirstOrDefault(),
                ClassName = _context.classes.Where(x => x.Id == s.ClassId).Select(z => z.ClassName).FirstOrDefault()

            }).ToListAsync();
            return View(labellist);
        }

        [Authorize(Roles = "Admin ,User")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var list = await _context.Labels.Where(s => s.ApplicationUser == user).Select(s => new LabelVM
            {
                LableID = s.LableID,
                FromName = s.FromName,
                ToName = s.ToName,
                FromCity = s.FromCity,
                ToCity = s.ToCity,
                FromState = s.FromState,
                ToState = s.ToState,
                Provider = s.Provider,
                Class = s.Class,
                TrackingNo = s.TrackingNo,
                UserId = s.UserId,
                UserName = s.ApplicationUser == null ? "" : s.ApplicationUser.UserName,
                ProviderName = _context.providers.Where(x => x.Id == s.ProviderID).Select(z => z.ProviderName).FirstOrDefault(),
                ClassName = _context.classes.Where(x => x.Id == s.ClassId).Select(z => z.ClassName).FirstOrDefault()
            }).ToListAsync();



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

            var labels = await _context.Labels
                .FirstOrDefaultAsync(m => m.LableID == id);
            if (labels == null)
            {
                return NotFound();
            }

            return View(labels);
        }
        [Authorize(Roles = "Admin ,User")]
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
                var user = await _userManager.GetUserAsync(HttpContext.User);
                labels.ApplicationUser = user;

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
        [Authorize(Roles = "Admin ,User")]
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
        [Authorize(Roles = "Admin ,User")]
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
