using AllUp.Data;
using Microsoft.EntityFrameworkCore;

namespace AllUp.Services;

public class LayoutService
{
    private readonly AllUpDbContext _context;
    
    public LayoutService(AllUpDbContext context)
    {
        _context = context;
    }

    public async Task<IDictionary<string, string>>  GetSettingsAsync() => await _context.Settings.Where(s => !s.IsDeleted).ToDictionaryAsync(s => s.Key, s => s.Value);
}
