using AllUp.Models;

namespace AllUp.Interfaces
{
    public interface ILayoutService
    {
        Task<IDictionary<string, string>> GetSettingsAsync();
        Task<IEnumerable<Category>> GetCategoriesAsync();
    }
}
