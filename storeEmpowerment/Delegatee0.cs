using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace storeEmpowerment
{ [DataContract]
   public class Delegatee0
    {   [DataMember]
        public string user { get; set; }
        [DataMember]
        public string access { get; set; }
    }
}
