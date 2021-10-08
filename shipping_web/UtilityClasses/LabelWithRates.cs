using Shipping_Label_App.Models;
using Shipping_Label_App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shipping_Label_App.UtilityClasses
{
    public class LabelWithRates
    {
        public LabelVM labels { get; set; }
        public ShipingRatesModel ShipingRatesModel { get; set; }
    }
}
