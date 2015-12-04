using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment.calendar
{
    [DataContract]
    public class Counting
    {
        [DataMember]
        public string LastCountedDate { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string LastGapScanDate { get; set; }
        [DataMember]
        public string LastGapScanTime { get; set; }
        [DataMember]
        public string LastGapScanStatus { get; set; }
    }
}
