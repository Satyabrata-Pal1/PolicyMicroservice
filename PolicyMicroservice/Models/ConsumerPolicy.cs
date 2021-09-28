using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyMicroservice.Models
{
    public class ConsumerPolicy
    {
        public int PolicyId { get; set; }
        public int ConsumerId { get; set; }
        public int AgentId { get; set; }
        public int BusinessId { get; set; }
        public int QuotesValue { get; set; }
      
        public bool IssueStatus { get; set; }
        public bool AcceptanceStatus { get; set; }
        public string PolicyStatus { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int CoveredSum { get; set; }
        public int Duration { get; set; }
        public string PaymentDetails { get; set; }
        public string PolicyType { get; set; }

    }
}
