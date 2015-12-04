using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment.calendar
{
    [DataContract]
    public class Sale
    {
        [DataMember]
        public double Expected { get; set; }
        [DataMember]
        public string ExpectedAsCasesAndSingles { get; set; }
        [DataMember]
        public int Actual { get; set; }
        [DataMember]
        public string ActualAsCasesAndSingles { get; set; }
        [DataMember]
        public string Type { get; set; }
    }
}
