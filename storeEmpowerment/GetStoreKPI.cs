using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    [DataContract]
    public class GetStoreKPI
    {
        public GetStoreKPI(string UserName, string access_token, string Retail_outlet_number)
        {
            this.UserName = UserName;
            this.access_token = access_token;
            this.retail_outlet_number = Retail_outlet_number;
        }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string access_token { get; set; }

        [DataMember]
        public string retail_outlet_number { get; set; }

    }
}
