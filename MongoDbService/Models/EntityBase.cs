using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbService.Models;

public class EntityBase : IEntityBase
{
    [BsonId]
    public ObjectId Id { get; set; }
}