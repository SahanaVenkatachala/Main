using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    [DataContract]
    public class populateResponse
    {
        [DataMember]
        public List<Stores> stores { get; set; }

        public populateResponse()
        {

        }
    }
}
