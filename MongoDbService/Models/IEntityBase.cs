using MongoDB.Bson;

namespace MongoDbService.Models;

public interface IEntityBase
{
    ObjectId Id { get; set; }
}