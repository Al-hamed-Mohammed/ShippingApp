using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shipping_Label_App.Models
{
    public class ProviderClasses
    {        
        public int ProviderID { get; set; }
        public Providers Providers { get; set; }
        public int ClassID { get; set; }
        public Classes Classes { get; set; }
    }
}
