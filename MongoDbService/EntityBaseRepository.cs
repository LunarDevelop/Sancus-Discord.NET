using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbService.Models;

namespace MongoDbService;

public class EntityBaseRepository<TEntityBase> : IEntityBaseRepository<TEntityBase>
    where TEntityBase : class, IEntityBase, new()
{
    private readonly IMongoCollection<TEntityBase> _collection;

    public EntityBaseRepository(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnString);
        var database = client.GetDatabase(settings.DatabaseName);
        _collection = database.GetCollection<TEntityBase>(typeof(TEntityBase).Name.Split("Model")[0]);
    }

    /// <summary>
    /// Adds a new entity to the collection
    /// </summary>
    /// <param name="entity">The entity to add</param>
    /// <returns>true or false depending on if it was a success</returns>
    public bool Insert(TEntityBase entity)
    {
        entity.Id = ObjectId.GenerateNewId();
        var task = _collection.InsertOneAsync(entity);
        task.Wait();
        return task.IsCompleted;
    }

    public bool Update(TEntityBase entity)
    {
        if (entity.Id == ObjectId.Empty)
            return Insert(entity);

        return _collection.ReplaceOne(x => x.Id == entity.Id, entity, new UpdateOptions() { IsUpsert = true })
            .ModifiedCount > 0;
    }

    public bool Delete(TEntityBase entity)
    {
        return _collection.DeleteOne(x => x.Id == entity.Id).DeletedCount > 0;
    }

    /// <summary>
    ///     Get all documents from a collection
    /// </summary>
    /// <returns>A list of documents in a collection</returns>
    public virtual List<TEntityBase> GetAll()
    {
        return _collection.AsQueryable().ToList();
    }

    public async Task<TEntityBase?> GetSingle(ObjectId id)
    {
        var obj = await _collection.FindAsync(x => x.Id == id);
        return obj.First();
    }

    public virtual long Count()
    {
        return _collection.Find(_ => true).CountDocuments();
    }

    public IList<TEntityBase> SearchFor(Expression<Func<TEntityBase, bool>> expression)
    {
        return _collection.AsQueryable().Where(expression.Compile()).ToList();
    }
}