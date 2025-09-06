using System.Linq.Expressions;

namespace TrainingManagementSystem.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task Add(T entity);
        Task Delete(T entity);
        Task Update(T entity);

        Task<T?> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    }
}
