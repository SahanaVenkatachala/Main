using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{

    [DataContract]
    class AppVersionRequest
    {
        public AppVersionRequest()
        {
            this.devicePlatform = "Windows";
            this.AppVersion = "1.73";
        }
        [DataMember]
        public string devicePlatform { get; set; }
        [DataMember]
        public string AppVersion { get; set; }
    }
}
