using Domain.SeedWork;

namespace Domain.Intefraces
{
    public interface IBaseRepository<T> where T : IAggregateRoot
    {
        public IQueryable<T> GetPaginatedAsync(int pageNumber, int pageSize);
        public Task AddAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync(T entity);
        public Task<int> GetTotalCountAsync();
    }
}
