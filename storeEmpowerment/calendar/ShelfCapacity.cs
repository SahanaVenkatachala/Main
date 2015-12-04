using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment.calendar
{
    [DataContract]
    public class ShelfCapacity
    {
        [DataMember]
        public int Capacity { get; set; }
        [DataMember]
        public string CapacityAsCasesAndSingles { get; set; }
        [DataMember]
        public string Facing { get; set; }
        [DataMember]
        public int CapacityForOffer { get; set; }
        [DataMember]
        public string CapacityForOfferAsCasesAndSingles { get; set; }
    }
}
