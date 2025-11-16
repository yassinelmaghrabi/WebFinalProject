namespace Api.Repsitories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entitiy);
        Task UpdateAsync(T entitiy);
        Task DeleteAsync(T entitiy);
        Task SaveChangesAsync();
    }
}
