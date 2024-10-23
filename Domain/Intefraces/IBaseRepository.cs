using Domain.SeedWork;

namespace Domain.Intefraces
{
    public interface IBaseRepository<T> where T : IAgregateRoot
    {
        public Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task AddAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync(T entity);
        public Task<int> GetTotalCountAsync();
    }
}
