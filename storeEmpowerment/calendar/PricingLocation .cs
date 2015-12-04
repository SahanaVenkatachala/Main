using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace storeEmpowerment.calendar
{
    [dataContract]
    public class PricingLocation
    {
        [DataMember]
        public string locType { get; set; }
        [DataMember]
        public string locRef { get; set; }
    }
}
