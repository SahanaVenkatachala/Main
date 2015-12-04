using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment.calendar
{
    [DataContract]
    class Calendar
    {
        [DataMember]
        public Product Product { get; set; }
        [DataMember]
        public Range Range { get; set; }
        [DataMember]
        public BookStock BookStock { get; set; }
        [DataMember]
        public Price Price { get; set; }
        [DataMember]
        public ShelfCapacity ShelfCapacity { get; set; }
        [DataMember]
        public List<Delivery> Deliveries { get; set; }
        [DataMember]
        public List<Sale> Sales { get; set; }
        [DataMember]
        public Counting Counting { get; set; }
        [DataMember]
        public List<ProductLocation> ProductLocation { get; set; }
        [DataMember]
        public List<Offer> Offers { get; set; }
    }
}
