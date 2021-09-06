﻿using Shipping_Label_App.UtilityClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shipping_Label_App.Models
{
    public class Labels
    {
        [Key]
        public int LableID { get; set; }
        public string FromCountry { get; set; }
        public string ToCountry { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Name")]
        public string FromName { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Name")]
        public string ToName { get; set; }

        [Required]
        [MaxLength(500)]
        [Display(Name = "Street")]
        public string FromStreet { get; set; }

        [Required]
        [MaxLength(500)]
        [Display(Name = "Street")]
        public string ToStreet { get; set; }

        [MaxLength(500)]
        [Display(Name = "Street 2 (optional)")]
        public string FromStreet2 { get; set; }

        [MaxLength(500)]
        [Display(Name = "Street 2 (optional)")]
        public string ToStreet2 { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "City")]
        public string FromCity { get; set; }

        [Required]
        [MaxLength(500)]
        [Display(Name = "City")]
        public string ToCity { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "Zip")]
        public string FromZip { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "Zip")]
        public string ToZip { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "State")]
        public string FromState { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "State")]
        public string ToState { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "Phone")]
        public string FromPhone { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "Phone")]
        public string ToPhone { get; set; }

        [Display(Name = "Providers")]
        public int ProviderID { get; set; }


        // Other secion fields here

        [Required]
        [Display(Name = "Weight (120 lbs max)")]
        public int Weight { get; set; }

        [Required]
        [Display(Name = "Class")]
        public int ClassId { get; set; }

        [Display(Name = "Signature Required")]
        public bool SignatureRequired { get; set; }
        public string Notes { get; set; }


        //Schedule section here
        [Display(Name = "Schedule Enabled")]
        public bool SheduleEnable { get; set; }
        [Display(Name = "Schedule Date (your local time)")]
        public DateTime SheduleDateTime { get; set; }

        // Tracking Details
        public string TrackingNo { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime DateModified { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public IEnumerable<States> StatesList { get; set; }

        [NotMapped]
        public IEnumerable<Country> CountriesList { get; set; }
        [NotMapped]
        public IEnumerable<Providers> Providers { get; set; }
        [NotMapped]
        public IEnumerable<Classes> Classes { get; set; }

        [NotMapped]
        public Providers Provider { get; set; }
        [NotMapped]
        public Classes Class { get; set; }

    }
}
