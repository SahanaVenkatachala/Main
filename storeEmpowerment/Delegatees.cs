using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace storeEmpowerment
{
    [DataContract]
    public class Delegatees
    {
        [DataMember]
        public Delegatee0 delegatee0 { get; set; }
        [DataMember]
        public Delegatee0 delegatee1 { get; set; }
        [DataMember]
        public Delegatee0 delegatee2 { get; set; }
        [DataMember]
        public Delegatee0 delegatee3 { get; set; }

    }
}
