using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    [DataContract]
    public class AppVersionResponse
    {
        public AppVersionResponse()
        {

        }

        [DataMember]
        public string LatestApplicationVersion { get; set; }

        [DataMember]
        public string MinimumSupportedVersion { get; set; }
      
        [DataMember]
        public string key { get; set; }

    }
}
