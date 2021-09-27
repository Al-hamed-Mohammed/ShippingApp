using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shipping_Label_App.UtilityClasses
{
    public class Country
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    public class RootCountryObject
    {
        public List<Country> Countries { get; set; }
    }
}
