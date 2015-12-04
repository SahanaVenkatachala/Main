using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    [DataContract]
    public class RoutineData
    {
        public RoutineData()
        {

        }
        [DataMember]
        public string Retail_Outlet_Number { get; set; }
        [DataMember]
        public string Store_Name { get; set; }

        [DataMember]
        public ThisQuarter last_quarter { get; set; }
        [DataMember]
        public ThisQuarter this_quarter { get; set; }

        [DataMember]

        public string cronTime { get; set; }

      
    }
}
