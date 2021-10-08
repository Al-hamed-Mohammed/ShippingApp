using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shipping_Label_App.UtilityClasses
{
    public class LabelObject
    {
        public origin_address origin_address { get; set; }
        public destination_address destination_address { get; set; }
        public string incoterms { get; set; }
        public insurance insurance { get; set; }
        public courier_selection courier_selection { get; set; }
        public shipping_settings shipping_settings { get; set; }

        public ICollection<parcels> parcels { get; set; }
    }

    public class origin_address
    {
        public string line_1 { get; set; }
        public string line_2 { get; set; }
        public string postal_code { get; set; }
        public string state { get; set; }
        public string city { get; set; }
    }
    public class destination_address
    {
        public string line_1 { get; set; }
        public string line_2 { get; set; }
        public string postal_code { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string country_alpha2 { get; set; }

    }

    public class insurance
    {
        public bool is_insured { get; set; }
        public double insured_amount { get; set; }
        public string insured_currency { get; set; }
    }

    public class courier_selection
    {
        public bool apply_shipping_rules { get; set; }
    }

    #region Shiping settings section
    public class units
    {
        public string weight { get; set; }
        public string dimensions { get; set; }
    }
    public class shipping_settings
    {
        public units units { get; set; }
        public string output_currency { get; set; }
    }
    #endregion

    #region Parcel section
    public class box
    {
        public int length { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class items
    {
        public int quantity { get; set; }
        public box dimensions { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public int actual_weight { get; set; }
        public string declared_currency { get; set; }
        public int declared_customs_value { get; set; }
    }

    public class parcels
    {
        public double total_actual_weight { get; set; }
        public box box { get; set; }

        public ICollection<items> items { get; set; }
    }

    #endregion
}
