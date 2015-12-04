using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    [DataContract]
    class LoginRequest
    {
        public LoginRequest(string access, string refresh, string username){
            DevicePlatform = "Windows 8.1";
            AppVersion = "1.73";
            UserName = username;
            access_token = access;
            refresh_token = refresh;
        }
        [DataMember]
        public string DevicePlatform { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string AppVersion { get; set; }
        [DataMember]
        public string access_token { get; set; }
        [DataMember]
        public string refresh_token { get; set; }

    }

}

