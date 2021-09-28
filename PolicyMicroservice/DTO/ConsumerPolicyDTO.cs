using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyMicroservice.DTO
{
    public class ConsumerPolicyDTO
    {
       
            //public int id { get; set; }
            public int ConsumerId { get; set; }
            public int BusinessId { get; set; }
            public int PropertyId { get; set; }
           
            /**public bool issueStatus { get; set; }
            public bool acceptanceStatus { get; set; }
            public string policyStatus { get; set; }
            public string effectiveDate { get; set; }
            public int coveredSum { get; set; }
            public int duration { get; set; }
            public string paymentDetails { get; set; }**/
        
    }
}
