using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace storeEmpowerment
{
    [DataContract]
    public class ThisQuarter
    {

        public ThisQuarter()
        {

        }
        [DataMember]
        public string sd_group_number { get; set; }
        [DataMember]
        public string overall_score { get; set; }
        [DataMember]
        public string overall_RAG { get; set; }
        [DataMember]
        public string metric_1 { get; set; }
        [DataMember]
        public string metric_1_RAG { get; set; }
        [DataMember]
        public string metric_1_description { get; set; }
        [DataMember]
        public string metric_2 { get; set; }
        [DataMember]
        public string metric_2_RAG { get; set; }
        [DataMember]
        public string metric_2_description { get; set; }
        [DataMember]
        public string metric_3 { get; set; }
        [DataMember]
        public string metric_3_RAG { get; set; }
        [DataMember]
        public string metric_3_description { get; set; }
        [DataMember]
        public string metric_4 { get; set; }
        [DataMember]
        public string metric_4_RAG { get; set; }
        [DataMember]
        public string metric_4_description { get; set; }
        [DataMember]
        public string metric_5 { get; set; }
        [DataMember]
        public string metric_5_RAG { get; set; }
        [DataMember]
        public string metric_5_description { get; set; }
        [DataMember]
        public string is_approved { get; set; }
        [DataMember]
        public string update_timestamp { get; set; }
    }
}
