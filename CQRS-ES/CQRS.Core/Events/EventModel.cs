using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace CQRS.Core.Events
{
    public class EventModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid AggegateIdentifier { get; set; }
        public string? AggregateType { get; set; }
        public int Version { get; set;}
        public string? EventType { get; set; }
        public BaseEvent? EventData { get; set; }
    }
}