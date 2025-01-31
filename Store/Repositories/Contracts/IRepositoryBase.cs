using System.Linq.Expressions;

namespace Repositories.Contracts
{
    public interface IRepositoryBase<T> 
    {
    IQueryable<T> FindAll(bool trackChanges);  
     T? FindByCondition(Expression<Func<T,bool>> Expression, bool trackChanges);

     void Create(T Entity);

     void Remove(T Entity);

     void Update(T Entity);
    }
}