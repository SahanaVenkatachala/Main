using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace storeEmpowerment.calendar
{
    [DataContract]
    public class PriceAPI
    {
        [DataMember]
        public string tpnb { get; set; }
        [DataMember]
        public string pricingDate { get; set; }
        [DataMember]
        public PricingLocation pricingLocation { get; set; }
        [DataMember]
        public List<Variant> variants { get; set; }
        [DataMember]
        public List<object> promotions { get; set; }
        [DataMember]
        public List<object> clearances { get; set; }
    }
}
