using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace storeEmpowerment
{
    [DataContract]
    class RefreshRequest
    {
        public RefreshRequest(String refresh)
        {
            refresh_token = refresh;
        }

        [DataMember]
        public string refresh_token { get; set; }
    }
}
