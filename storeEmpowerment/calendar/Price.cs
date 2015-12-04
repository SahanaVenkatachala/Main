using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment.calendar
{
    [DataContract]
    public class Price
    {
        [DataMember]
        public double SellingPrice { get; set; }
    }
}
