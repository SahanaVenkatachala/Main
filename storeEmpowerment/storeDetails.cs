using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
   public class storeDetails:IComparable
    {
       public storeDetails()
       {

       }
       public storeDetails(string name, string number)
       {
           this.storeName = name;
           this.storeNumber = number;
       }
       public string storeName { get; set; }

       public string storeNumber { get; set; }


      public int CompareTo(object obj)
       {
           storeDetails details = (storeDetails)obj;
           int result = this.storeName.CompareTo(details.storeName);
           return result;
       }
    }
}
