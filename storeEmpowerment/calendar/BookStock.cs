using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment.calendar
{
    [DataContract]
    public class BookStock
    {
        [DataMember]
        public string UOM { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public string QuantityInCasesAndSingles { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string StockDate { get; set; }
        [DataMember]
        public string OutOfStockShortReason { get; set; }
        [DataMember]
        public string OutOfStockLongReason { get; set; }
    }
}
