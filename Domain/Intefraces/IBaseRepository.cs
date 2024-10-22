using Domain.SeedWork;

namespace Domain.Intefraces
{
    public interface IBaseRepository<T> where T : IAgregateRoot
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
