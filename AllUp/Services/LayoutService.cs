using AllUp.Data;
using Microsoft.EntityFrameworkCore;
using AllUp.Interfaces;
using AllUp.Models;

namespace AllUp.Services;

public class LayoutService : ILayoutService
{
    private readonly AllUpDbContext _context;

    public LayoutService(AllUpDbContext context)
    {
        _context = context;
    }

    public async Task<IDictionary<string, string>> GetSettingsAsync() => await _context.Settings.AsNoTracking().Where(s => !s.IsDeleted).ToDictionaryAsync(s => s.Key, s => s.Value);

    public async Task<IEnumerable<Category>> GetCategoriesAsync() => await _context.Categories.AsNoTracking().Where(c => !c.IsDeleted && c.IsMain).Include(c => c.Children).ToListAsync();
}
