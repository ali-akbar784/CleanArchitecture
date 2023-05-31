namespace CleanArch.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(long id);
        Task<int> AddAsync(T entity);
        Task<string> UpdateAsync(T entity);
    }
}
