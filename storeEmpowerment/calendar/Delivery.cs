using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment.calendar
{
    [DataContract]
    public class Delivery
    {
        [DataMember]
        public string RequestDeliveryDate { get; set; }
        [DataMember]
        public string RequestDeliveryTime { get; set; }
        [DataMember]
        public string ActualDeliveryDate { get; set; }
        [DataMember]
        public string ActualDeliveryTime { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public string QuantityAsCasesAndSingles { get; set; }
    }

}
