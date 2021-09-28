using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyMicroservice.Models
{
    public class PolicyMaster
    {
        public int id { get; set; }
        public PropertyTypes propertyType { get; set; }
        public string consumerType { get; set; }
        public int assuredSum { get; set; }
        public int tenure { get; set; }

        public int businessValue { get; set; }
        public int propertyValue { get; set; }
        public string baseLocation { get; set; }
        public string type { get; set; }

        public enum PropertyTypes
        {
            Building = 0,
            FactoryEquipment = 1,
            PropertyInTransit = 2,
            ColdStorage = 3
        }
    }
}
