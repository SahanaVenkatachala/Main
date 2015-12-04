using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    public class productDetails
    {

        public productDetails(string description, string imageurl, string ean, string color)
        {
            this.Description = description;
            this.ImageURL = imageurl;
            this.color = color;
            this.EAN = ean;

        }
        public string Description { get; set; }

        public string EAN { get; set; }
              
        public string ImageURL { get; set; }

        public string color { get; set; }

    }
}
