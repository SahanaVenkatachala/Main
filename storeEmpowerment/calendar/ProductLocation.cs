using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment.calendar
{
    [DataContract]
    public class ProductLocation
    {
        [DataMember]
        public string Aisle { get; set; }
        [DataMember]
        public string AisleSide { get; set; }
        [DataMember]
        public string Module { get; set; }
        [DataMember]
        public string Floor { get; set; }
        [DataMember]
        public string Shelf_Reference { get; set; }
        [DataMember]
        public string Sequence_Number { get; set; }
    }
}
