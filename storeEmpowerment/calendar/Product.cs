using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment.calendar
{
    [DataContract]
    public class Product
    {
        [DataMember]
        public string EAN { get; set; }
        [DataMember]
        public string TPNB { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string UnitSize { get; set; }
        [DataMember]
        public string SellByWeight { get; set; }
    }
}
