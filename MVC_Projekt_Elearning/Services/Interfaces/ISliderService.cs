using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.ViewModels.Sliders;
using System.Reflection.Metadata;

namespace MVC_Projekt_Elearning.Services.Interfaces
{
    public interface ISliderService
    {
        Task<IEnumerable<SliderVM>> GetAllAsync(int? take = null);
        Task<bool> ExistAsync(string title);
        Task CreateAsync(Slider slider);
        Task<Slider> GetByIdAsync(int id);
        Task DeleteAsync(Slider slider);
        Task EditAsync();
        Task<bool> ExistByIdAsync(int id, string title);
    }
}
