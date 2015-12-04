using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    [DataContract]
    public class SearchByNameResponse
    {
        public SearchByNameResponse()
        {
            
        }

        [DataMember]
        public string Shelf { get; set; }
        [DataMember]
        public List<Product> Products { get; set; }
    }
}
