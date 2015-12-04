using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
     [DataContract]
   public  class TeamAccessData
    {
         public TeamAccessData()
         {

         }
         public TeamAccessData(string delegatee, string retail_outlet_number, string calAccess)
         {
             this.delegatee = delegatee;
             this.retail_outlet_number = App.localSettings.Values["stores"].ToString();
             this.calAccess = calAccess;
             System.Diagnostics.Debug.WriteLine(this.calAccess);
         }

         [DataMember]
        public string delegatee{get;set;}
         
         [DataMember]
         public string retail_outlet_number { get; set; }

         [DataMember]
         public string calAccess { get; set; }

        
    }
}
