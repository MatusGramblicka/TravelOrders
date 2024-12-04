using System.Linq.Expressions;

namespace Interface.DatabaseAccess;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}