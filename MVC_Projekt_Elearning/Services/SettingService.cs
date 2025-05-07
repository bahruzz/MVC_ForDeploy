using Microsoft.EntityFrameworkCore;
using MVC_Projekt_Elearning.Data;
using MVC_Projekt_Elearning.Services.Interfaces;
using NuGet.Configuration;

namespace MVC_Projekt_Elearning.Services
{
    public class SettingService : ISettingService
    {
        private readonly AppDbContext _context;
        public SettingService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<string, string>> GetAllAsync()
        {
            return await _context.Settings.ToDictionaryAsync(m => m.Key, m => m.Value);
        }
    }
}
