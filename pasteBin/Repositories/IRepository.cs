using System.Linq;

namespace pasteBin.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}