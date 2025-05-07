using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.ViewModels.Abouts;
using MVC_Projekt_Elearning.ViewModels.Informations;

namespace MVC_Projekt_Elearning.Services.Interfaces
{
    public interface IAboutService
    {
        Task<IEnumerable<AboutVM>> GetAllAsync(int? take = null);
        Task<bool> ExistAsync(string title);
        Task CreateAsync(About about);
        Task<About> GetByIdAsync(int id);
        Task DeleteAsync(About about);
        Task EditAsync();
        Task<bool> ExistByIdAsync(int id, string title);
        Task<About> GetAboutAsync();
    }
}
