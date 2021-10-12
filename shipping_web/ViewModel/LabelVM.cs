using Shipping_Label_App.CustomerValidatorClass;
using Shipping_Label_App.Models;
using Shipping_Label_App.UtilityClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shipping_Label_App.ViewModel
{
    public class LabelVM
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
        public string ProviderName { get; set; }


        // Other secion fields here

        [Required]
        [RequiredGreaterThanZeroForDouble]
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
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ClassName { get; set; }


        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

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

        [NotMapped]
        [Display(Name = "From Address")]
        public string FromAddress
        {
            get
            {
                return FromStreet + ", " + FromCity + ", " + FromState + ", " + FromZip;
            }
        }

        [NotMapped]
        [Display(Name = "To Address")]
        public string ToAddress
        {
            get
            {
                return ToStreet + ", " + ToCity + ", " + ToState + ", " + ToZip;
            }
        }
        [NotMapped]
        public string RomeName { get; set; }

        [Required]
        [RequiredGreaterThanZeroForDouble]
        public double Length { get; set; }

        [Required]
        [RequiredGreaterThanZeroForDouble]
        public double Width { get; set; }

        [Required]
        [RequiredGreaterThanZeroForDouble]
        public double Height { get; set; }

        public string courier_name { get; set; }
        public double total_charge { get; set; }
    }
}
