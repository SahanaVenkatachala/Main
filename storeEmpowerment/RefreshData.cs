using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    [DataContract]
    class RefreshData
    {
        [DataMember]
        public string refresh_token { get; set; }
    }
}
