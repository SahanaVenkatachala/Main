using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    [DataContract]
    public class Stores
    {
        [DataMember]
        public string Retail_Outlet_Number { get; set; }
        [DataMember]
        public string store_manager_name { get; set; }
        [DataMember]
        public string approval { get; set; }
        [DataMember]
        public string Store_Name { get; set; }
        public Stores()
        {

        }
    }
}
