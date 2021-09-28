using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyMicroservice.DTO
{
    public class ConsumerDTO
    {
        public int ConsumerId { get; set; }
        public int AgentId { get; set; }
        public int PropertyValue { get; set; }
        public int BusinessValue { get; set; }
        public int BusinessId { get; set; }
        public int PropertyId { get; set; }
        public enum PropertyTypes
        {
            Building = 0,
            FactoryEquipment = 1,
            PropertyInTransit = 2,
            ColdStorage = 3
        }
        public PropertyTypes PropertyType { get; set; }
    }
}
