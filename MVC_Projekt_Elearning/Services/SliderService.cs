using Microsoft.EntityFrameworkCore;
using MVC_Projekt_Elearning.Data;
using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.Services.Interfaces;
using MVC_Projekt_Elearning.ViewModels.Sliders;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace MVC_Projekt_Elearning.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;

        public SliderService(AppDbContext context)
        {
            _context= context;
            
        }

        public async Task CreateAsync(Slider slider)
        {
            await _context.AddAsync(slider);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Slider slider)
        {
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(string title)
        {
            return await _context.Sliders.AnyAsync(m => m.Title.Trim() == title.Trim());
        }

        public async  Task<bool> ExistByIdAsync(int id, string title)
        {
            return await _context.Sliders.AnyAsync(m => m.Title.Trim() == title.Trim() && m.Id != id);
        }

        public async Task<IEnumerable<SliderVM>> GetAllAsync(int? take = null)
        {
            IEnumerable<Slider> sliders;
            if (take is null)
            {
                sliders = await _context.Sliders.ToListAsync();
            }
            else
            {
                sliders = await _context.Sliders.Take((int)take).ToListAsync();
            }

            return sliders.Select(m => new SliderVM { Id = m.Id, Title = m.Title, Description = m.Description, Image = m.Image, CreatedDate = m.CreatedDate.ToString("MM.dd.yyyy") });
        }

        public async Task<Slider> GetByIdAsync(int id)
        {
            var a = await _context.Sliders.Where(m => m.Id == id).FirstOrDefaultAsync();
            return a;
        }
    }
}
