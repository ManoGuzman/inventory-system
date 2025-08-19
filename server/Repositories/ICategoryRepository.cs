namespace Server.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<string>> GetAllCategoriesAsync();
        Task<bool> CategoryExistsAsync(string category);
        Task<int> GetProductCountByCategoryAsync(string category);
    }
}
