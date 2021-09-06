using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string FromName { get; set; }
        public string ToName { get; set; }

        public string FromStreet { get; set; }

        public string ToStreet { get; set; }

        public string FromStreet2 { get; set; }
        public string ToStreet2 { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public string FromZip { get; set; }
        public string ToZip { get; set; }
        public string FromState { get; set; }
        public string ToState { get; set; }
        public string FromPhone { get; set; }
        public string ToPhone { get; set; }
    }
}
