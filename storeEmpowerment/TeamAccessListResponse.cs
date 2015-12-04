using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    [DataContract]
    public class TeamAccessListResponse
    {
        public TeamAccessListResponse()
        {

        }

        [DataMember]
        public Delegatees delegatees { get; set; }
        [DataMember]
        public bool storeManager { get; set; }
        [DataMember]
        public string storeManagerName { get; set; }
        

        

       

       /* public TeamAccessListResponse(Delegatee delegatee)
        {
            int i=0;
            // TODO: Complete member initialization
            this.delegatees.delegatee1 = delegatee.ToString();
            i++;
            Debug.WriteLine(delegatee);
        }

        public TeamAccessListResponse(string delegatees)
        {

            this.delegatees.delegatee1 = delegatees;
        }

       */

       

        

        
    }
}
