using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Messages;
using MongoDB.Bson.Serialization.Attributes;

namespace CQRS.Core.Events
{    
    
    public class BaseEvent : Message
    {
        protected BaseEvent(string type)
        {
            Type = type;
        }
        
        public int Version { get; set; }
        public string? Type { get; set; }
    }
}