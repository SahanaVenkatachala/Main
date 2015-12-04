using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace storeEmpowerment.calendar
{
    [dataContract]
    public class Variant
    {
        [DataMember]
        public string tpnc { get; set; }
        [DataMember]
        public string currency { get; set; }
        [DataMember]
        public string sellingUOM { get; set; }
        [DataMember]
        public string regPrice { get; set; }
        [DataMember]
        public object promoPrice { get; set; }
        [DataMember]
        public object clrPrice { get; set; }
        [DataMember]
        public string authPrice { get; set; }
    }
}
