using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.ViewModels.Informations;
using MVC_Projekt_Elearning.ViewModels.Sliders;

namespace MVC_Projekt_Elearning.Services.Interfaces
{
    public interface IInformationService
    {
        Task<IEnumerable<InformationVM>> GetAllAsync(int? take = null);
        Task<bool> ExistAsync(string title);
        Task CreateAsync(Information information);
        Task<Information> GetByIdAsync(int id);
        Task DeleteAsync(Information information);
        Task EditAsync();
        Task<bool> ExistByIdAsync(int id, string title);
    }
}
