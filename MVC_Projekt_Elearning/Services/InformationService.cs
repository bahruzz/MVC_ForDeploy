using Microsoft.EntityFrameworkCore;
using MVC_Projekt_Elearning.Data;
using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.Services.Interfaces;
using MVC_Projekt_Elearning.ViewModels.Informations;
using MVC_Projekt_Elearning.ViewModels.Sliders;

namespace MVC_Projekt_Elearning.Services
{
    public class InformationService : IInformationService
    {
        private readonly AppDbContext _context;

        public InformationService(AppDbContext context)
        {
            _context = context;

        }
        public async Task CreateAsync(Information information)
        {
            await _context.AddAsync(information);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Information information)
        {
            _context.Informations.Remove(information);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(string title)
        {
            return await _context.Informations.AnyAsync(m => m.Title.Trim() == title.Trim());
        }

        public async Task<bool> ExistByIdAsync(int id, string title)
        {
            return await _context.Informations.AnyAsync(m => m.Title.Trim() == title.Trim() && m.Id != id);
        }

        public async Task<IEnumerable<InformationVM>> GetAllAsync(int? take = null)
        {
            IEnumerable<Information> informations;
            if (take is null)
            {
                informations = await _context.Informations.ToListAsync();
            }
            else
            {
                informations = await _context.Informations.Take((int)take).ToListAsync();
            }

            return informations.Select(m => new InformationVM { Id = m.Id, Title = m.Title, Description = m.Description, Image = m.Image, CreatedDate = m.CreatedDate.ToString("MM.dd.yyyy") });
        }

        public async Task<Information> GetByIdAsync(int id)
        {
            var a = await _context.Informations.Where(m => m.Id == id).FirstOrDefaultAsync();
            return a;
        }
    }
}
