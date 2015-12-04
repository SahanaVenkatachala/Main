using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment.calendar
{
    [DataContract]
    public class Offer
    {
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int OfferShelfCapacity { get; set; }
        [DataMember]
        public string OfferShelfCapacityAsCasesAndSingles { get; set; }
    }
}
