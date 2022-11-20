using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbService.Models;

namespace MongoDbService;

public interface IEntityBaseRepository<TEntityBase>
    where TEntityBase : class, IEntityBase, new()
{
    bool Insert(TEntityBase entity);
    bool Update(TEntityBase entity);
    bool Delete(TEntityBase entity);
    long Count();
    List<TEntityBase> GetAll();
    IList<TEntityBase> SearchFor(Expression<Func<TEntityBase, bool>> expression);
    Task<TEntityBase?> GetSingle(ObjectId id);
}