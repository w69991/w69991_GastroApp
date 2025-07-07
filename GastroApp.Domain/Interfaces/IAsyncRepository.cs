namespace GastroApp.Domain.Interfaces;

//Uniwersalne repozytorium asynchroniczne, zawiera podstawowe operacje CRUD dla dowolnej encji
//Dziedziczonej po BaseEntity
public interface IAsyncRepository<T> where T : GastroApp.Common.BaseEntity
{
    Task<T?>            GetByIdAsync(Guid id);
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<T>             AddAsync(T entity);
    Task                UpdateAsync(T entity);
    Task                DeleteAsync(T entity);
}
