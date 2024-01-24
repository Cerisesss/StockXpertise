using SqlKata.Execution;

namespace StockXpertise.Models
{
    public abstract class BaseModel<T> where T : BaseModel<T>, new()
    {
        protected static QueryFactory QueryBuilder => App.GetService<QueryFactory>();

        protected virtual string TableName => typeof(T).Name.ToLower();

        public static T Insert(T entity)
        {
            QueryBuilder.Query(entity.TableName).AsInsert(entity);
            return entity;
        }


        public static IEnumerable<T> GetAll()
        {
            var instance = new T();
            return QueryBuilder.Query(instance.TableName).Get<T>();
        }
    }
}
