
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    [DataContract]
    public class Product
    {
        public Product()
        {

        }
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Price { get; set; }

        [DataMember]
        public string EAN { get; set; }

        [DataMember]
        public string ShelfName { get; set; }

        [DataMember]
        public string ImageURL { get; set; }

        [DataMember]
        public string LargeImageURL { get; set; }
    }
}
