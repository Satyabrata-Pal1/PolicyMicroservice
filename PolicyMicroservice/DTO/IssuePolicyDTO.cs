using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyMicroservice.DTO
{
    public class IssuePolicyDTO
    {
        public int AgentId { get; set; }
        public int ConsumerId { get; set; }
        public int BusinessId { get; set; }
        public int PropertyId { get; set; }
        public int PolicyId { get; set; }
        public string PaymentDetails { get; set; }
        public bool AcceptanceStatus { get; set; }
    }
}
