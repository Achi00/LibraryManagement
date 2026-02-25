namespace LibraryManagement.Application.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        // no explicit update method needed!!!
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        void Add(T entity);
        void Delete(T entity);
    }
}
