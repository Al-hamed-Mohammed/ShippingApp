using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shipping_Label_App.UtilityClasses
{
    public class ShipingRatesModel
    {
        public string status { get; set; }
        public List<Rate> rates { get; set; }
    }

    public class Discount
    {
        public int amount { get; set; }
        public object code { get; set; }
        public object percentage { get; set; }
        public object expires_at { get; set; }
    }

    public class Rate
    {
        public string courier_id { get; set; }
        public string courier_name { get; set; }
        public int min_delivery_time { get; set; }
        public int max_delivery_time { get; set; }
        public double value_for_money_rank { get; set; }
        public double delivery_time_rank { get; set; }
        public string currency { get; set; }
        public double shipment_charge { get; set; }
        public double fuel_surcharge { get; set; }
        public double remote_area_surcharge { get; set; }
        //public RemoteAreaSurcharges remote_area_surcharges { get; set; }
        public double oversized_surcharge { get; set; }
        public double additional_services_surcharge { get; set; }
        public double residential_full_fee { get; set; }
        public double residential_discounted_fee { get; set; }
        public double shipment_charge_total { get; set; }
        public double warehouse_handling_fee { get; set; }
        public double insurance_fee { get; set; }
        public double sales_tax { get; set; }
        public double provincial_sales_tax { get; set; }
        public double ddp_handling_fee { get; set; }
        public double import_tax_charge { get; set; }
        public double import_tax_non_chargeable { get; set; }
        public double import_duty_charge { get; set; }
        public double total_charge { get; set; }
        public bool is_above_threshold { get; set; }
        public string incoterms { get; set; }
        public int estimated_import_tax { get; set; }
        public int estimated_import_duty { get; set; }
        public double minimum_pickup_fee { get; set; }
        public string available_handover_options { get; set; }
        public int tracking_rating { get; set; }
        public double easyship_rating { get; set; }
        public object courier_remarks { get; set; }
        public string payment_recipient { get; set; }
        public Discount discount { get; set; }
        public string description { get; set; }
        public string full_description { get; set; }
    }
}
