using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    [DataContract]
    public class LoginResponse
    {
        [DataMember]
        public string access_token;

        [DataMember]
        public string refresh_token;

        [DataMember]
        public int expires_in;

        public string userName;

        public LoginResponse()
        {

        }
    }
}
