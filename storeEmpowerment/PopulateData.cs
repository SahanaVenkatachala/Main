using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
   [DataContract]
    class PopulateData
    {
       public PopulateData(string userName, string accessToken, int expiryDate)
       {
           this.UserName = userName;
           this.access_token = accessToken;
           this.expires_in = expiryDate;
       }

       [DataMember]
       public string UserName { get; set; }

       [DataMember]
       public string access_token { get; set; }

       [DataMember]
       public int expires_in { get; set; }
    }
}
