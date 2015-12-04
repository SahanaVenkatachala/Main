using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    [DataContract]
    class TeamAccessDeleteData
    {
        [DataMember]
        public string delegatee { get; set; }
        [DataMember]
        public string retail_outlet_number { get; set; }
        public TeamAccessDeleteData(string delegatee, string retail_outlet_number)
        {
            this.delegatee = delegatee;
            this.retail_outlet_number = retail_outlet_number;
        }
    }
}