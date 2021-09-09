using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shipping_Label_App.Models
{
    public class Providers
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        [Display(Name = "Provider")]
        public string ProviderName { get; set; }

        [Required]
        public int ShipmentCost { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public virtual ICollection<ProviderClasses> ProviderClasses { get; set; }

        [NotMapped]
        public IEnumerable<Classes> Classes { get; set; }

    }
}
